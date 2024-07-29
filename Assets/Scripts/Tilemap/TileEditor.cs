using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.EventSystems;

public class TileEditor : Singleton<TileEditor>
{
    //Credit to https://www.youtube.com/@VelvaryGames for a lot of this code.
    
    PlayerInput playerInput;

    Camera camera;

    [SerializeField] private Tilemap previewMap, defaultMap, terrainMap;
    private TileBase tileBase;
    private Preset selectedObj;

    private Vector2 mousePosition;
    private Vector3Int currentGridPosition;
    private Vector3Int previousGridPosition;

    private bool holdActive;
    private Vector3Int holdStartPosition;
    private BoundsInt area;

    private City city;

    protected override void Awake()
    {
        base.Awake();

        city = City.GetInstance();

        playerInput = new PlayerInput();

        camera = Camera.main;
    }

    private void Update()
    {
        if (selectedObj != null)
        {
            //Setting pos as a Vector3 causes issues
            Vector2 pos = camera.ScreenToWorldPoint(mousePosition);

            Vector3Int gridPos = previewMap.WorldToCell(pos);

            if (gridPos == currentGridPosition) return;
            
            previousGridPosition = currentGridPosition;
            currentGridPosition = gridPos;

            UpdatePreview();

            if (!holdActive) return;
            HandleDrawing();
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
        playerInput.Gameplay.MouseLeftClick.started += OnLeftClick;
        playerInput.Gameplay.MouseLeftClick.canceled += OnLeftClick;
        playerInput.Gameplay.MouseRightClick.performed += OnRightClick;
        playerInput.Gameplay.MousePosition.performed += OnMouseMove;

        playerInput.Gameplay.KeyboardEsc.performed += OnKeyboardEsc;
    }

    private void OnDisable()
    {
        playerInput.Disable();

        playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
        playerInput.Gameplay.MouseLeftClick.started += OnLeftClick;
        playerInput.Gameplay.MouseLeftClick.canceled += OnLeftClick;
        playerInput.Gameplay.MouseRightClick.performed -= OnRightClick;
        playerInput.Gameplay.MousePosition.performed -= OnMouseMove;
    }

    private Preset SelectedObj
    {
        set
        {
            //Do not set this to the name of the setter, will crash unity editor lol
            holdActive = false;
            previewMap.ClearAllTiles();
            
            selectedObj = value;

            tileBase = selectedObj != null ? selectedObj.TileBase : null;

            UpdatePreview();
        }
    }

    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (selectedObj != null && !EventSystem.current.IsPointerOverGameObject() && PlacementAllowed())
        {
            if (ctx.phase == InputActionPhase.Started)
            {
                if (ctx.interaction is SlowTapInteraction)
                {
                    holdActive = true;
                }

                holdStartPosition = currentGridPosition;
                HandleDrawing();
            }
            else
            {
                if (holdActive)
                {
                    holdActive = false;
                    HandleDrawingRelease();
                }
            }
        }
    }

    private void OnRightClick(InputAction.CallbackContext ctx)
    {
        RemoveItem();
    }

    private void OnKeyboardEsc(InputAction.CallbackContext ctx)
    {
        SelectedObj = null;
    }

    public void ObjectSelected(Preset obj)
    {
        SelectedObj = obj;
    }

    private void UpdatePreview()
    {
        previewMap.SetTile(previousGridPosition, null);
        previewMap.SetTile(currentGridPosition, tileBase);
    }

    private void HandleDrawing()
    {
        if (selectedObj == null) return;

        switch (selectedObj.PlacementType)
        {
            case PlacementType.Line:
                break;
            case PlacementType.Rectangle:
                RenderRectangle();
                
                break;
            case PlacementType.Single: default:
                var cityCheck = city.NewTile(currentGridPosition, selectedObj);
                
                if (!cityCheck) break;
                
                DrawItem(currentGridPosition, tileBase);
                
                SelectedObj = null;
                
                break;
        }
    }

    private void HandleDrawingRelease()
    {
        switch (selectedObj.PlacementType)
        {
            case PlacementType.Line:
                break;
            case PlacementType.Rectangle:
                DrawArea(defaultMap);
                previewMap.ClearAllTiles();
                break;
        }
    }

    private void RenderRectangle()
    {
        previewMap.ClearAllTiles();
        
        //area.xMin = currentGridPosition.x < holdStartPosition.x ? currentGridPosition.x : holdStartPosition.x;
        //area.xMin = currentGridPosition.x > holdStartPosition.x ? currentGridPosition.x : holdStartPosition.x;
        //area.yMin = currentGridPosition.y < holdStartPosition.y ? currentGridPosition.y : holdStartPosition.y;
        //area.yMin = currentGridPosition.y > holdStartPosition.y ? currentGridPosition.y : holdStartPosition.y;
        
        area.xMin = Mathf.Min(currentGridPosition.x,holdStartPosition.x);
        area.xMax = Mathf.Max(currentGridPosition.x,holdStartPosition.x);
        area.yMin = Mathf.Min(currentGridPosition.y,holdStartPosition.y);
        area.yMax = Mathf.Max(currentGridPosition.y,holdStartPosition.y);
        
        DrawArea(previewMap);
    }

    private void DrawArea(Tilemap tilemap)
    {
        for (var x = area.xMin; x <= area.xMax; x++)
        {
            for (var y = area.yMin; y <= area.yMax; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), tileBase);
            }
        }
    }

    public void DrawItem(Vector3Int gridPosition, TileBase tile)
    {
        defaultMap.SetTile(gridPosition, tile);

        //Required for our network tile rules
        defaultMap.RefreshAllTiles();
    }

    private void RemoveItem()
    {
        if(!defaultMap.HasTile(currentGridPosition)) return;
        
        city.RemoveTile(currentGridPosition);

        defaultMap.SetTile(currentGridPosition, null);

        //Required for out network tile rules
        defaultMap.RefreshAllTiles();
    }

    private bool PlacementAllowed()
    {
        bool hasTerrainTile = terrainMap.HasTile(currentGridPosition - new Vector3Int(1, 1, 0));
        bool hasBuildingTile = defaultMap.HasTile(currentGridPosition);

        if (hasTerrainTile && !hasBuildingTile)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}