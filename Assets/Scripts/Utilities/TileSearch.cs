using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSearch
{
    public static int GetServiceScore(TileType tileType, IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        //Get nearby tiles here to see if any are a given building type
        //% score should depend on how close the building is
        //If building is present, return 100 for this score for now
        
        return cityTiles.Any(tile => tile.Value.TileType == tileType) ? 100 : 0;
    }

    public static bool HasTileType(TileType tileType, IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        return cityTiles.Any(tile => tile.Value.TileType == tileType);
    }
}