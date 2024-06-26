using UnityEngine;

public static class MapExtensions
{
    public static Vector3Int North(this Vector3Int vector)
    {
        return vector + Vector3Int.up;
    }

    public static Vector3Int East(this Vector3Int vector)
    {
        return vector + Vector3Int.left;
    }

    public static Vector3Int South(this Vector3Int vector)
    {
        return vector + Vector3Int.down;
    }

    public static Vector3Int West(this Vector3Int vector)
    {
        return vector + Vector3Int.right;
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