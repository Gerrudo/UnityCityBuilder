using System.Collections.Generic;
using UnityEngine;

public static class TilemapExtension
{
    //The methods of each vector may look strange but this is because we are using an Z as Y tilemap.
    public static Vector3Int North(this Vector3Int vector)
    {
        return vector + Vector3Int.left;
    }

    public static Vector3Int East(this Vector3Int vector)
    {
        return vector + Vector3Int.up;
    }

    public static Vector3Int South(this Vector3Int vector)
    {
        return vector + Vector3Int.right;
    }

    public static Vector3Int West(this Vector3Int vector)
    {
        return vector + Vector3Int.down;
    }

    public static Vector3Int[] Neighbours(this Vector3Int vector)
    {
        return new Vector3Int[4]
        {
        vector.North(),
        vector.East(),
        vector.South(),
        vector.West()
        };
    }
    
    //This helper method was required due to BoundsInt.allPositionsWithin not returning if any point is set to 0.
    public static IEnumerable<Vector3Int> AllPositionsWithin2D(BoundsInt area)
    {
        for (var x = area.xMin; x <= area.xMax; x++)
        {
            for (var y = area.yMin; y <= area.yMax; y++)
            {
                yield return new Vector3Int(x, y, 0);
            }
        }
    }
}