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
    public bool IsPowered { get; set; }

    
    public WaterTower(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public int GenerateWater(int water)
    {
        const int maxWater = 20000;
        const int generatedWater = 2000;
        
        //TODO: Replace with a TileSearch
        //if (!IsPowered) return water;
        if (water >= maxWater) return water;

        return water + generatedWater;
    }

    public int ConsumeWater(int water)
    {
        return water;
    }
    
    public int GeneratePower(int power)
    {
        return power;
    }

    public int ConsumePower(int power)
    {
        var powerConsumed = 200;

        IsPowered = powerConsumed > power;
        
        power -= powerConsumed;

        return power;
    }
}