using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class WaterTower : Building, IEmployer, IPower, IWater
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public override bool IsActive { get; set; }
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
    
    public override void UpdateBuildingStatus()
    {
        var flags = new List<bool> {IsConnectedToRoad, IsPowered};

        var hasFalse =  flags.Contains(false);

        IsActive = !hasFalse;
    }
    
    public int GenerateWater(int water)
    {
        if (!IsConnectedToRoad) return water;
        
        const int maxWater = 20000;
        
        if (water >= maxWater) return water;

        return water + maxWater;
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
        if (!IsConnectedToRoad) return power;
        
        const int powerConsumed = 200;

        IsPowered = powerConsumed < power;
        
        power -= powerConsumed;

        return power;
    }
}