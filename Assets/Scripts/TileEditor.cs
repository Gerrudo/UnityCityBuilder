using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TileEditor : Singleton<TileEditor>
{
    PlayerInput playerInput;

    Camera _camera;

    [SerializeField] private Tilemap previewMap, defaultMap, terrainMap;
    private TileBase tileBase;
    private PlaceableTile selectedObj;

    private Vector2 mousePosition;
    private Vector3Int currentGridPosition;
    private Vector3Int previousGridPosition;

    City city;

    protected override void Awake()
    {
        base.Awake();

        city = City.GetInstance();

        playerInput = new PlayerInput();

        _camera = Camera.main;
    }

    private void Update()
    {
        if (selectedObj != null)
        {
            //Setting pos as a Vector3 causes issues
            Vector2 pos = _camera.ScreenToWorldPoint(mousePosition);

            Vector3Int gridPos = previewMap.WorldToCell(pos);

            if (gridPos != currentGridPosition)
            {
                previousGridPosition = currentGridPosition;
                currentGridPosition = gridPos;

                UpdatePreview();
            }
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
        playerInput.Gameplay.MouseRightClick.performed += OnRightClick;
        playerInput.Gameplay.MousePosition.performed += OnMouseMove;
    }

    private void OnDisable()
    {
        playerInput.Disable();

        playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
        playerInput.Gameplay.MouseRightClick.performed -= OnRightClick;
        playerInput.Gameplay.MousePosition.performed -= OnMouseMove;
    }

    private PlaceableTile SelectedObj
    {
        set
        {
            //Do not set this to the name of the setter, will crash unity editor lol
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
            HandleDrawing();
        }
    }

    private void OnRightClick(InputAction.CallbackContext ctx)
    {
        SelectedObj = null;
    }

    public void ObjectSelected(PlaceableTile obj)
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
        bool cityCheck = city.NewTile(currentGridPosition, selectedObj);

        if (cityCheck)
        {
            DrawItem(currentGridPosition, tileBase);
        }
    }

    public void DrawItem(Vector3Int gridPosition, TileBase tile)
    {
        defaultMap.SetTile(gridPosition, tile);

        //Required for out network tile rules
        defaultMap.RefreshAllTiles();
    }

    private bool PlacementAllowed()
    {
        if (!terrainMap.HasTile(currentGridPosition) || defaultMap.HasTile(currentGridPosition))
        {
            Debug.Log("Cannot place tile on water or other buildings.");
            return false;
        }
        else
        {
            return true;
        }
    }
}