using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tool", menuName = "Create Tool")]
public class TilemapTool : BuildingPreset
{
    [field: SerializeField] public ToolType ToolType { get; private set; }

    private ToolController toolController;

    public void Use(Vector3Int position, Tilemap tilemap)
    {
        //TODO: Better way to do this?
        toolController = ToolController.GetInstance();
        
        switch (ToolType)
        {
            case ToolType.Bulldozer:
                toolController.Bulldozer(position, tilemap);
                break;
            case ToolType.Query:
                toolController.Query(position, tilemap);
                break;
            case ToolType.None:
            default:
                break;
        }
    }
}
