using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.EventSystems;

public class TileEditor : Singleton<TileEditor>
{
    //Credit to https://www.youtube.com/@VelvaryGames for a lot of this code.
    
    private PlayerInput playerInput;

    private new Camera camera;

    [SerializeField] private Tilemap previewMap, defaultMap, terrainMap;
    private TileBase tileBase;
    private Preset selectedObj;

    private Vector2 mousePosition;
    private Vector3Int currentGridPosition;
    private Vector3Int previousGridPosition;
    
    private bool isPointerOverGameObject;

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
        isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
        
        if (!selectedObj) return;
        
        //Setting pos as a Vector3 causes issues
        Vector2 pos = camera.ScreenToWorldPoint(mousePosition);

        var gridPos = previewMap.WorldToCell(pos);

        if (gridPos == currentGridPosition) return;
            
        previousGridPosition = currentGridPosition;
        currentGridPosition = gridPos;

        UpdatePreview();

        if (!holdActive) return;
        HandleDrawing();
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

            tileBase = selectedObj ? selectedObj.TileBase : null;

            UpdatePreview();
        }
    }

    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (selectedObj && !isPointerOverGameObject && PlacementAllowed())
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
                if (!holdActive) return;
                
                holdActive = false;
                HandleDrawingRelease();
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
        if (!selectedObj) return;

        switch (selectedObj.PlacementType)
        {
            case PlacementType.Line:
                RenderLine();
                
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
            case PlacementType.Rectangle:
                DrawArea(defaultMap);
                
                previewMap.ClearAllTiles();
                
                break;
        }
    }

    private void RenderRectangle()
    {
        previewMap.ClearAllTiles();
        
        area.xMin = Mathf.Min(currentGridPosition.x,holdStartPosition.x);
        area.xMax = Mathf.Max(currentGridPosition.x,holdStartPosition.x);
        area.yMin = Mathf.Min(currentGridPosition.y,holdStartPosition.y);
        area.yMax = Mathf.Max(currentGridPosition.y,holdStartPosition.y);
        
        DrawArea(previewMap);
    }

    private void RenderLine()
    {
        previewMap.ClearAllTiles();

        float diffX = Mathf.Abs(currentGridPosition.x - holdStartPosition.x);
        float diffY = Mathf.Abs(currentGridPosition.y - holdStartPosition.y);

        var isLineHorizontal = diffX >= diffY;

        if (isLineHorizontal)
        {
            area.xMin = Mathf.Min(currentGridPosition.x,holdStartPosition.x);
            area.xMax = Mathf.Max(currentGridPosition.x,holdStartPosition.x);
            area.yMin = holdStartPosition.y;
            area.yMax = holdStartPosition.y;
        }
        else
        {
            area.xMin = currentGridPosition.x;
            area.xMax = currentGridPosition.x;
            area.yMin = Mathf.Min(currentGridPosition.y,holdStartPosition.y);
            area.yMax = Mathf.Max(currentGridPosition.y,holdStartPosition.y);
        }
        
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

        //Required for our network tile rules
        defaultMap.RefreshAllTiles();
    }

    private bool PlacementAllowed()
    {
        bool hasTerrainTile = terrainMap.HasTile(currentGridPosition - new Vector3Int(1, 1, 0));
        bool hasBuildingTile = defaultMap.HasTile(currentGridPosition);

        return hasTerrainTile && !hasBuildingTile;
    }
}