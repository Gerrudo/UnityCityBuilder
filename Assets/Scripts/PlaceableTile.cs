using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Road,
    Residential,
    Commerical,
    Industrial,
    Generator,
    WaterTower,
    CoalMine
}

[CreateAssetMenu(fileName = "PlaceableTile", menuName = "PlaceableTile")]
public class PlaceableTile : ScriptableObject
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