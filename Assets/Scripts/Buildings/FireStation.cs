using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class FireStation : Building, IEmployer, IPower, IWater, IEarnings
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    
    public FireStation(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public int GenerateWater()
    {
        return 0;
    }

    public int ConsumeWater()
    {
        return Jobs.Count * 4;
    }
    
    public int GeneratePower()
    {
        return 0;
    }

    public int ConsumePower()
    {
        return Jobs.Count * 4;
    }
    
    public int GenerateEarnings()
    {
        return 0;
    }

    public int ConsumeEarnings()
    {
        return Jobs.Count * 10;
    }
}