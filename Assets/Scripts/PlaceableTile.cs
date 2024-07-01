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
    [SerializeField] private TileType tileType;
    [SerializeField] private TileBase tileBase;
    [SerializeField] private int moneyPerDay;
    [SerializeField] private int costToBuild;

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

    public int MoneyPerDay
    {
        get
        {
            return moneyPerDay;
        }
    }

    public int CostToBuild {
        get
        {
            return costToBuild;
        }
    }
}