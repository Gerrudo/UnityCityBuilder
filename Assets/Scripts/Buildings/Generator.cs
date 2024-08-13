using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Generator : Building, IEmployer, IPower, IWater, IEarnings
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public override bool IsActive { get; set; }
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    public bool IsWatered { get; set; }

    
    public Generator(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public override void UpdateBuildingStatus()
    {
        var flags = new List<bool> {IsConnectedToRoad, IsWatered};

        var hasFalse =  flags.Contains(false);

        IsActive = !hasFalse;
    }
    
    public int GenerateWater(int water)
    {
        return water;
    }

    public int ConsumeWater(int water)
    {
        if (!IsConnectedToRoad) return 0;
        
        const int waterConsumed = 200;

        IsWatered = waterConsumed < water;
        
        water -= waterConsumed;

        return water;
    }
    
    public int GeneratePower(int power)
    {
        if (!IsConnectedToRoad) return power;
        
        const int maxPower = 50000;
        
        if (power >= maxPower) return power;

        return power + maxPower;
    }

    public int ConsumePower(int power)
    {
        return power;
    }
    
    public int GenerateEarnings()
    {
        return 0;
    }

    public int ConsumeEarnings()
    {
        if (!IsActive) return 0;
        
        return 1000;
    }
}