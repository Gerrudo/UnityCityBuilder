using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolController : Singleton<ToolController>
{
    private City city;
    
    public void Bulldozer(Vector3Int position, Tilemap tilemap)
    {
        //TODO: See why we need to get an instance of our city each time we use these methods.
        city = City.GetInstance();
        
        if(!tilemap.HasTile(position)) return;
        city.RemoveTile(position);
        
        tilemap.SetTile(position, null);
        tilemap.RefreshAllTiles();
    }

    public void Query(Vector3Int position, Tilemap tilemap)
    {
        //TODO: See why we need to get an instance of our city each time we use these methods.
        city = City.GetInstance();
        
        if (city.CityTiles.TryGetValue(position, out var building))
        {
            Debug.Log(building.TileType);
        }
    }
}
