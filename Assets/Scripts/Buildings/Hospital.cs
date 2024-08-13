using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Hospital : Building, IEmployer, IPower, IWater, IEarnings
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public override bool IsActive { get; set; }
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    public bool IsPowered { get; set; }
    public bool IsWatered { get; set; }

    
    public Hospital(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public override void UpdateBuildingStatus()
    {
        var flags = new List<bool> {IsConnectedToRoad, IsPowered, IsWatered};

        var hasFalse =  flags.Contains(false);

        IsActive = !hasFalse;
    }
    
    public int GenerateWater(int water)
    {
        return water;
    }

    public int ConsumeWater(int water)
    {
        if (!IsConnectedToRoad) return water;
        
        var waterConsumed = Jobs.Count * 4;

        IsWatered = waterConsumed < water;
        
        water -= waterConsumed;

        return water;
    }
    
    public int GeneratePower(int power)
    {
        return power;
    }

    public int ConsumePower(int power)
    {
        if (!IsConnectedToRoad) return power;
        
        var powerConsumed = Jobs.Count * 4;

        IsPowered = powerConsumed < power;
        
        power -= powerConsumed;

        return power;
    }
    
    public int GenerateEarnings()
    {
        return 0;
    }

    public int ConsumeEarnings()
    {
        if (!IsActive) return 0;
        
        return Jobs.Count * 10;
    }
}