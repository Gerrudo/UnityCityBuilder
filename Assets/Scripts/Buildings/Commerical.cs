using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Commercial : Building, IEmployer, IGrowable, IPower, IWater, IGoods, IEarnings, IApproval
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public override bool IsActive { get; set; }
    public TileBase Level1TilBase { get; set; }

    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    public bool IsPowered { get; set; }
    public bool IsWatered { get; set; }

    
    public Commercial(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        Level1TilBase = buildingPreset.Level1TilBase;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public override void UpdateBuildingStatus()
    {
        var flags = new List<bool> {IsConnectedToRoad, IsPowered, IsWatered};

        var hasFalse =  flags.Contains(false);

        IsActive = !hasFalse;
    }
    
    public bool CanUpgrade()
    {
        if (!IsActive || Jobs.Count <= 10 || TileBase == Level1TilBase) return false;
        TileBase = Level1TilBase;
        return true;
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
        if (!IsActive) return 0;
        
        return Jobs.Count * 10;
    }

    public int ConsumeEarnings()
    {
        return 0;
    }

    public int GenerateGoods()
    {
        return 0;
    }

    public int ConsumeGoods()
    {
        if (!IsActive) return 0;
        
        return Jobs.Count * 2;
    }

    public double GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        const float employmentWeight = 0.6f;
        const float fireStationWeight = 0.2f;
        const float isPoweredWeight = 0.1f;
        const float isWateredWeight = 0.1f;

        var isPoweredScore = IsPowered ? 100 : 0;
        var isWateredScore = IsPowered ? 100 : 0;

        return (GetEmploymentScore() * employmentWeight) +
                            (TileSearch.GetServiceScore(TileType.Fire, cityTiles) * fireStationWeight) +
                            (isPoweredScore * isPoweredWeight) +
                            (isWateredScore * isWateredWeight);
    }

    private int GetEmploymentScore()
    {
        var score = Calculations.GetPercentage(Jobs.Count, MaxEmployees);
        return (int)score;
    }
}