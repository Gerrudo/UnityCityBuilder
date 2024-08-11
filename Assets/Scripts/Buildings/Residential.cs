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

    public Residential(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        Level1TilBase = buildingPreset.Level1TilBase;
        MaxPopulation = buildingPreset.MaxPopulation;
        
        Residents = new List<Citizen>();
    }

    public bool CanUpgrade()
    {
        if (!IsConnectedToRoad || Residents.Count <= 10|| TileBase == Level1TilBase) return false;
        TileBase = Level1TilBase;
        return true;
    }
    
    public int GenerateWater()
    {
        return 0;
    }

    public int ConsumeWater()
    {
        return Residents.Count * 4;
    }
    
    public int GeneratePower()
    {
        return 0;
    }

    public int ConsumePower()
    {
        return Residents.Count * 4;
    }

    public int GenerateEarnings()
    {
        return Residents.Count * 5;
    }

    public int ConsumeEarnings()
    {
        return 0;
    }
    
    public float GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        const float employmentWeight = 0.4f;
        const float fireStationWeight = 0.2f;
        const float policeStationWeight = 0.2f;
        const float hospitalWeight = 0.2f;

        var approvalScore= (GetEmploymentScore() * employmentWeight) +
               (TileSearch.GetServiceScore(TileType.Fire, cityTiles) * fireStationWeight) +
               (TileSearch.GetServiceScore(TileType.Police, cityTiles) * policeStationWeight) +
               (TileSearch.GetServiceScore(TileType.Medical, cityTiles) * hospitalWeight);

        return approvalScore;
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