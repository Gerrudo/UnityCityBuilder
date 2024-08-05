using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Generator : Building, IEmployer, IPower, IWater, IEarnings
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    
    public Generator(Preset buildingPreset)
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
        return 500;
    }
    
    public int GeneratePower()
    {
        return 25000;
    }

    public int ConsumePower()
    {
        return 0;
    }
    
    public int GenerateEarnings()
    {
        return 0;
    }

    public int ConsumeEarnings()
    {
        return 1000;
    }
}