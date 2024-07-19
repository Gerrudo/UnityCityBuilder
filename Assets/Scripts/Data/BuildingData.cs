using UnityEngine.Tilemaps;

public class BuildingData
{
    public TileType TileType { get; set; }
    public TileBase Level1TilBase { get; set; }
    public int CurrentPopulation { get; set; }
    public int MaxPopulation { get; set; }
    public int Taxes { get; set; }
    public int Expenses { get; set; }
    public int BuildingLevel { get; set; }
    public bool IsConnectedToRoad { get; set; }
    public int Unemployed { get; set; }
    public int Employees { get; set; }
    public int MaxEmployees { get; set; }
    public int PowerConsumption { get; set; }
    public int PowerProduction { get; set; }
    public int WaterConsumption { get; set; }
    public int WaterProduction { get; set; }
    
    public int GoodsConsumption { get; set; }
    public int GoodsProduction { get; set; }
}