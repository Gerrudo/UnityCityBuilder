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
    //Private setter is required for serialised fields.
    [field: SerializeField]
    public TileType TileType { get; private set; }

    [field: SerializeField]
    public TileBase TileBase { get; private set; }

    [field: SerializeField]
    public TileBase Level1Tilebase { get; private set; }

    [field: SerializeField]
    public int Taxes { get; private set; }

    [field: SerializeField]
    public int CostToBuild { get; private set; }

    [field: SerializeField]
    public int MaxPopulation { get; private set; }

    public int CurrentPopulation { get; set; }

    public bool IsConnectedToRoad { get; set; }
    public bool IsWatered { get; set; }
    public bool IsPowered { get; set; }
}