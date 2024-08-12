using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Commercial : Building, IEmployer, IGrowable, IPower, IWater, IGoods, IEarnings, IApproval
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
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
    
    public bool CanUpgrade()
    {
        if (!IsConnectedToRoad || Jobs.Count <= 10 || TileBase == Level1TilBase) return false;
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

        IsWatered = waterConsumed > water;
        
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

        IsPowered = powerConsumed > power;
        
        power -= powerConsumed;

        return power;
    }
    
    public int GenerateEarnings()
    {
        if (!IsConnectedToRoad) return 0;
        
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
        if (!IsConnectedToRoad) return 0;
        
        return Jobs.Count * 2;
    }

    public float GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        const float employmentWeight = 0.8f;
        const float fireStationWeight = 0.2f;

        var approvalScore = (GetEmploymentScore() * employmentWeight) +
                            (TileSearch.GetServiceScore(TileType.Fire, cityTiles) * fireStationWeight);

        return approvalScore;
    }

    private int GetEmploymentScore()
    {
        var score = Calculations.GetPercentage(Jobs.Count, MaxEmployees);
        return (int)score;
    }
}