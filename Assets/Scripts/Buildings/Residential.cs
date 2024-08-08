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
        return Residents.Count * 10;
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
               (GetServiceScore(TileType.Fire, cityTiles) * fireStationWeight) +
               (GetServiceScore(TileType.Police, cityTiles) * policeStationWeight) +
               (GetServiceScore(TileType.Medical, cityTiles) * hospitalWeight);

        return approvalScore;
    }

    private int GetEmploymentScore()
    {
        var employed = Residents.Count(resident => resident.IsEmployed);
        
        var score = Calculations.GetPercentage(employed, Residents.Count);
        
        return (int)score;
    }
    
    private int GetServiceScore(TileType tileType, IReadOnlyDictionary<Vector3Int, Building> cityTiles)
    {
        //Get nearby tiles here to see if any are a given building type
        //% score should depend on how close the building is
        //If building is present, return 100 for this score for now
        
        var score = cityTiles.Any(tile => tile.Value.TileType == tileType) ? 100 : 0;

        return score;
    }
}