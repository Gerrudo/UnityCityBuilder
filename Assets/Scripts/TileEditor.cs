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
    ZoneTileBase selectedObj;

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

    private ZoneTileBase SelectedObj
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

    public void ObjectSelected(ZoneTileBase obj)
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
    }
}