using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine;

public class Residential : Building, IGrowable, IResidence, IWater, IEarnings, IApproval, IPower
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public int MaxPopulation { get; set; }
    public List<Citizen> Residents { get; set; }
    public TileBase Level1TilBase { get; set; }
    public bool IsPowered { get; set; }
    public bool IsWatered { get; set; }
    public override bool IsActive { get; set; }

    public Residential(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        Level1TilBase = buildingPreset.Level1TilBase;
        MaxPopulation = buildingPreset.MaxPopulation;
        
        Residents = new List<Citizen>();
    }

    public override void UpdateBuildingStatus()
    {
        var flags = new List<bool> {IsConnectedToRoad, IsPowered, IsWatered};

        var hasFalse =  flags.Contains(false);

        IsActive = !hasFalse;
    }

    public bool CanUpgrade()
    {
        if (!IsActive || Residents.Count <= 10|| TileBase == Level1TilBase) return false;
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

        var waterConsumed = Residents.Count * 4;

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
        
        var powerConsumed = Residents.Count * 4;

        IsPowered = powerConsumed < power;
        
        power -= powerConsumed;

        return power;
    }

    public int GenerateEarnings()
    {
        if (!IsConnectedToRoad) return 0;
        
        return Residents.Count * 5;
    }

    public int ConsumeEarnings()
    {
        return 0;
    }
    
    public float GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
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

    public int GetPopulationMultiplier(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        //Must be < 0, multiplying by 0 will always return 0 so population will never grow.
        var multiplier = 1;

        if (TileSearch.HasTileType(TileType.Medical, cityTiles)) multiplier++;
        if (TileSearch.HasTileType(TileType.Fire, cityTiles)) multiplier++;
        if (TileSearch.HasTileType(TileType.Police, cityTiles)) multiplier++;

        return multiplier;
    }

    private int GetEmploymentScore()
    {
        var employed = Residents.Count(resident => resident.IsEmployed);
        var score = Calculations.GetPercentage(employed, Residents.Count);
        
        return (int)score;
    }
}