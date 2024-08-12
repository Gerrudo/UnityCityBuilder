using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Industrial : Building, IEmployer, IGrowable, IPower, IWater, IGoods, IEarnings, IApproval
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public TileBase Level1TilBase { get; set; }

    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    public bool IsPowered { get; set; }
    public bool IsWatered { get; set; }

    
    public Industrial(BuildingPreset buildingPreset)
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
        var powerConsumed = Jobs.Count * 4;

        IsPowered = powerConsumed > power;
        
        power -= powerConsumed;

        return power;
    }
    
    public int GenerateEarnings()
    {
        return Jobs.Count * 10;
    }

    public int ConsumeEarnings()
    {
        return 0;
    }

    public int GenerateGoods()
    {
        return Jobs.Count * 10;
    }

    public int ConsumeGoods()
    {
        return 0;
    }
    
    public float GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        const float employmentWeight = 0.4f;
        const float policeStationWeight = 0.2f;

        var approvalScore = (GetEmploymentScore() * employmentWeight) +
                            (TileSearch.GetServiceScore(TileType.Police, cityTiles) * policeStationWeight);

        return approvalScore;
    }

    private int GetEmploymentScore()
    {
        var score = Calculations.GetPercentage(Jobs.Count, MaxEmployees);
        return (int)score;
    }
}