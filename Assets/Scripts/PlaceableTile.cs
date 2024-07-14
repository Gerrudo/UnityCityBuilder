using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Road,
    Residential,
    Commercial,
    Industrial,
    Generator,
    WaterTower
}

[CreateAssetMenu(fileName = "PlaceableTile", menuName = "PlaceableTile")]
public class PlaceableTile : ScriptableObject
{
    //Private setter is required for serialised fields.
    [field: SerializeField] public TileType TileType { get; private set; }

    [field: SerializeField] public TileBase TileBase { get; private set; }

    [field: SerializeField] public TileBase Level1Tilebase { get; private set; }

    [field: SerializeField] public int CostToBuild { get; private set; }

    [field: SerializeField] public int MaxPopulation { get; private set; }
    
    public int PowerDemand { get; set; }
    
    [field: SerializeField] public int PowerProduction { get; set; }
    
    public int WaterDemand { get; set; }
    
    [field: SerializeField] public int WaterProduction { get; set; }

    public int CurrentPopulation { get; set; }

    public bool IsConnectedToRoad { get; set; }
    
    public bool IsPowered { get; set; }
    
    public bool IsWatered { get; set; }

    public int Happiness { get; set; }
    
    public int Taxes { get; set; }
    
    [field: SerializeField] public int Expenses { get; set; }
    
    public int Workers { get; set; }
    
    public int MaxWorkers { get; set; }
}