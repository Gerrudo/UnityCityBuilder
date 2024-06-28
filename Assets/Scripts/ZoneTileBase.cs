using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Residential,
    Commerical,
    Industrial
}

[CreateAssetMenu(fileName = "Zone Tile Base", menuName = "Create Zone Tile Base")]
public class ZoneTileBase : ScriptableObject
{
    [SerializeField] TileType tileType;
    [SerializeField] TileBase tileBase;

    public TileBase TileBase
    {
        get
        {
            return tileBase;
        }
    }

    public TileType TileType
    {
        get
        {
            return tileType;
        }
    }
}