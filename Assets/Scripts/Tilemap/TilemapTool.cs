using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tool", menuName = "Create Tool")]
public class TilemapTool : BuildingPreset
{
    [SerializeField] private ToolType toolType;

    private ToolController toolController;

    private void Awake()
    {
        toolController = ToolController.GetInstance();
    }

    public void Use(Vector3Int position, Tilemap tilemap)
    {
        //TODO: Better way to do this?

        
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
