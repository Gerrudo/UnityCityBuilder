using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolController : Singleton<ToolController>
{
    private City city;

    protected override void Awake()
    {
        base.Awake();
        
        city = City.GetInstance();
    }

    public void Bulldozer(Vector3Int position, Tilemap tilemap)
    {
        if(!tilemap.HasTile(position)) return;
        city.RemoveTile(position);
        
        tilemap.SetTile(position, null);
        tilemap.RefreshAllTiles();
    }

    public void Query(Vector3Int position, Tilemap tilemap)
    {
        if (city.CityTiles.TryGetValue(position, out var building))
        {
            //Debug.Log(building.TileType);
        }
    }
}
