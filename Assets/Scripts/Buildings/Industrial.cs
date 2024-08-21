using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Industrial : Building, IEmployable, IGrowable, IPowerable, IWaterable, ITaxable, IApprovable
{
    public TileBase Level1TilBase { get; set; }
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    public int WaterConsumption { get; set; }
    public int PowerConsumption { get; set; }
    public int TaxRevenue { get; set; }
    public int GoodsProduction { get; set; }
    
    public Industrial(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        Level1TilBase = buildingPreset.Level1TilBase;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public override void UpdateBuilding(CityData cityData)
    {
        WaterConsumption = ConsumeWater();
        PowerConsumption = ConsumePower();
        TaxRevenue = GenerateTaxes();
        GoodsProduction = GenerateGoods();

        cityData.Water -= WaterConsumption;
        cityData.Power -= PowerConsumption;
        cityData.Earnings += TaxRevenue;
        cityData.Goods += GoodsProduction;
    }
    
    public override void UpdateBuildingStatus(CityData cityData)
    {
        IsWatered = WaterConsumption < cityData.Water;
        IsPowered = PowerConsumption < cityData.Power;
        
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

    public int ConsumeWater()
    {
        if (!IsConnectedToRoad) return 0;
        
        return Jobs.Count * 4;
    }
    
    public int ConsumePower()
    {
        if (!IsConnectedToRoad) return 0;
        
        return Jobs.Count * 4;
    }
    
    public int GenerateTaxes()
    {
        if (!IsActive) return 0;
        
        return Jobs.Count * 10;
    }

    public int GenerateGoods()
    {
        if (!IsActive) return 0;
        
        return Jobs.Count * 10;
    }
    
    public double GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        const float employmentWeight = 0.6f;
        const float policeStationWeight = 0.2f;
        const float isPoweredWeight = 0.1f;
        const float isWateredWeight = 0.1f;

        var isPoweredScore = IsPowered ? 100 : 0;
        var isWateredScore = IsPowered ? 100 : 0;

        return (GetEmploymentScore() * employmentWeight) +
               (TileSearch.GetServiceScore(TileType.Police, cityTiles) * policeStationWeight) +
               (isPoweredScore * isPoweredWeight) +
               (isWateredScore * isWateredWeight);
    }

    private int GetEmploymentScore()
    {
        var score = Calculations.GetPercentage(Jobs.Count, MaxEmployees);
        return (int)score;
    }
}