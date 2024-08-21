using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine;

public class Residential : Building, IPowerable, IWaterable, IGrowable, ITaxable, IApprovable
{
    public readonly int MaxPopulation;
    public List<Citizen> Residents { get; set; }
    public TileBase Level1TilBase { get; set; }
    public int WaterConsumption { get; set; }
    public int PowerConsumption { get; set; }
    public int TaxRevenue { get; set; }

    public Residential(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        Level1TilBase = buildingPreset.Level1TilBase;
        MaxPopulation = buildingPreset.MaxPopulation;
        
        Residents = new List<Citizen>();
    }

    public override void UpdateBuilding(CityData cityData)
    {
        WaterConsumption = ConsumeWater();
        PowerConsumption = ConsumePower();
        TaxRevenue = GenerateTaxes();

        cityData.Water -= WaterConsumption;
        cityData.Power -= PowerConsumption;
        cityData.Earnings += TaxRevenue;
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
        if (!IsActive || Residents.Count <= 10 || TileBase == Level1TilBase) return false;
        TileBase = Level1TilBase;
        return true;
    }

    public int ConsumeWater()
    {
        if (!IsConnectedToRoad) return 0;
        
        return Residents.Count * 4;
    }

    public int ConsumePower()
    {
        if (!IsConnectedToRoad) return 0;
        
        return Residents.Count * 4;
    }

    public int GenerateTaxes()
    {
        if (!IsActive) return 0;
        
        return Residents.Count * 5;
    }
    
    public double GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        const float employmentWeight = 0.5f;
        const float fireStationWeight = 0.1f;
        const float policeStationWeight = 0.1f;
        const float hospitalWeight = 0.1f;
        const float isPoweredWeight = 0.1f;
        const float isWateredWeight = 0.1f;

        var isPoweredScore = IsPowered ? 100 : 0;
        var isWateredScore = IsPowered ? 100 : 0;

        return (GetEmploymentScore() * employmentWeight) +
               (TileSearch.GetServiceScore(TileType.Fire, cityTiles) * fireStationWeight) +
               (TileSearch.GetServiceScore(TileType.Police, cityTiles) * policeStationWeight) +
               (TileSearch.GetServiceScore(TileType.Medical, cityTiles) * hospitalWeight) +
               (isPoweredScore * isPoweredWeight) +
               (isWateredScore * isWateredWeight);
    }
    
    private int GetEmploymentScore()
    {
        var employed = Residents.Count(resident => resident.IsEmployed);
        var score = Calculations.GetPercentage(employed, Residents.Count);
        
        return (int)score;
    }

    public int GetPopulationMultiplier(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        //Must be < 0, multiplying by 0 will always return 0 so population will never grow.
        var multiplier = 1;

        if (TileSearch.HasTileType(TileType.Medical, cityTiles)) multiplier++;
        if (TileSearch.HasTileType(TileType.Fire, cityTiles)) multiplier++;
        if (TileSearch.HasTileType(TileType.Police, cityTiles)) multiplier++;

        return multiplier;
    }
}