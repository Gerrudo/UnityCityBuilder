using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class WaterTower : Building, IEmployer, IPower, IWater
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    
    public WaterTower(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public int GenerateWater()
    {
        return 20000;
    }

    public int ConsumeWater()
    {
        return 0;
    }
    
    public int GeneratePower()
    {
        return 0;
    }

    public int ConsumePower()
    {
        return 200;
    }
}