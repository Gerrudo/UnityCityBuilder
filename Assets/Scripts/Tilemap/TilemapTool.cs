using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tool", menuName = "Create Tool")]
public class TilemapTool : BuildingPreset
{
    [SerializeField] private ToolType toolType;

    private ToolController toolController;

    public void Use(Vector3Int position, Tilemap tilemap)
    {
        //TODO: See if we can only call this once rather than each time Use() is called, might be hard though as it's a scriptable object.
        //Also don't remove this, if you place it in awake it'll result in a different instance of ToolController. It's retarded I know.
        toolController = ToolController.GetInstance();
        
        switch (toolType)
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
