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
}