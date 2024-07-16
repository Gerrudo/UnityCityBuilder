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
    public int CurrentWorkers { get; set; }
    public int MaxWorkers { get; set; }
}