using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class QueryTool : Singleton<QueryTool>
{
    [SerializeField] private Tilemap defaultMap;
    private PlayerInput playerInput;
    private new Camera camera;
    public bool IsQueryToolActive { get; set; }
    private Vector2 mousePosition;
    private Vector3Int currentGridPosition;
    private City city;
    
    protected override void Awake()
    {
        base.Awake();
        
        playerInput = new PlayerInput();
        
        camera = Camera.main;
        
        city = City.GetInstance();
    }
    
    private void Update()
    {
        if (!IsQueryToolActive) return;

        Vector2 pos = camera.ScreenToWorldPoint(mousePosition);

        currentGridPosition = defaultMap.WorldToCell(pos);
    }
    
    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
        playerInput.Gameplay.MousePosition.performed += OnMouseMove;

        playerInput.Gameplay.KeyboardEsc.performed += OnKeyboardEsc;
    }

    private void OnDisable()
    {
        playerInput.Disable();

        playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
        playerInput.Gameplay.MousePosition.performed -= OnMouseMove;
    }
    
    private void OnMouseMove(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }
    
    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        if (!IsQueryToolActive || !defaultMap.HasTile(currentGridPosition)) return;
        
        var building = city.GetBuildingData(currentGridPosition);
        
        Debug.Log($"Building clicked was: {building.TileType}");
    }

    private void OnKeyboardEsc(InputAction.CallbackContext ctx)
    {
        IsQueryToolActive = false;
    }
}