using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TileEditor : Singleton<TileEditor>
{
    PlayerInput playerInput;

    Camera _camera;

    [SerializeField] Tilemap previewMap, defaultMap;
    TileBase tileBase;
    PlaceableTile selectedObj;

    Vector2 mousePos;
    Vector3Int curGridPos;
    Vector3Int prevGridPos;

    protected override void Awake()
    {
        base.Awake();

        playerInput = new PlayerInput();

        _camera = Camera.main;
    }

    private void Update()
    {
        if (selectedObj != null)
        {
            //Setting pos as a Vector3 causes issues
            Vector2 pos = _camera.ScreenToWorldPoint(mousePos);

            Vector3Int gridPos = previewMap.WorldToCell(pos);

            //We create a new vector 3 to place the buildings on 1 for the Z axis, we need to move our map down by 1 instead in the future.
            gridPos = new Vector3Int(gridPos.x, gridPos.y, 1);

            if (gridPos != curGridPos)
            {
                prevGridPos = curGridPos;
                curGridPos = gridPos;

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
        mousePos = ctx.ReadValue<Vector2>();
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (selectedObj != null && !EventSystem.current.IsPointerOverGameObject())
        {
            DrawItem();
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
        previewMap.SetTile(prevGridPos, null);
        previewMap.SetTile(curGridPos, tileBase);
    }

    private void DrawItem()
    {
        defaultMap.SetTile(curGridPos, tileBase);

        //Required for out network tile rules
        defaultMap.RefreshAllTiles();
    }
}