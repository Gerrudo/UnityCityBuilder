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
    public bool IsWatered { get; set; }

    
    public Generator(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    public int GenerateWater(int water)
    {
        return water;
    }

    public int ConsumeWater(int water)
    {
        var waterConsumed = 200;

        IsWatered = waterConsumed > water;
        
        water -= waterConsumed;

        return water;
    }
    
    public int GeneratePower(int power)
    {
        const int maxPower = 50000;
        const int generatedPower = 5000;
        
        //TODO: Replace with a TileSearch
        //if (!IsWatered) return power;
        if (power >= maxPower) return power;

        return power + generatedPower;
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
        return 1000;
    }
}