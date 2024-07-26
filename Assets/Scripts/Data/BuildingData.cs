using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class BuildingData
{
    public TileType TileType { get; set; }
    public TileBase Level1TilBase { get; set; }
    public int CurrentPopulation { get; set; }
    public List<Citizen> Residents { get; set; }
    public int MaxPopulation { get; set; }
    public int Taxes { get; set; }
    public int Expenses { get; set; }
    public int BuildingLevel { get; set; }
    public bool IsConnectedToRoad { get; set; }
    public int Unemployed { get; set; }
    public int Employees { get; set; }
    public List<Guid> Jobs { get; set; }
    public int MaxEmployees { get; set; }
    public int PowerInput { get; set; }
    public int PowerOutput { get; set; }
    public int WaterInput { get; set; }
    public int WaterOutput { get; set; }
    public int GoodsInput { get; set; }
    public int GoodsOutput { get; set; }
}