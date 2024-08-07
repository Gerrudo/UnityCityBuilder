using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;

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

    public void CanUpgrade()
    {
        if (IsConnectedToRoad && Residents.Count > 10)
        {
            TileBase = Level1TilBase;
        }
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
    
    public float GetApprovalScore()
    {
        //TODO: Weight should be count of total values divided by 10.
        var employmentWeight = 2.5f;
        var fireStationWeight = 2.5f;
        var policeStationWeight = 2.5f;
        var hospitalWeight = 2.5f;

        return (GetEmploymentScore() * employmentWeight) +
               (GetServiceScore(TileType.Fire) * fireStationWeight) +
               (GetServiceScore(TileType.Police) * policeStationWeight) +
               (GetServiceScore(TileType.Medical) * hospitalWeight);
    }

    private double CalculatePercentage(double part, double whole) => (part / whole) * 100;

    private int GetEmploymentScore()
    {
        //Create percentage based on employed citizens/unemployed citizens
        var unemployed = Residents.Count(resident => !resident.IsEmployed);
        
        var score = CalculatePercentage(unemployed, Residents.Count);
        
        return (int)score;
    }
    
    private int GetServiceScore(TileType tileType)
    {
        //Get nearby tiles here to see if any are a given building type
        //If building is present, return 100 for this score for now
        return 0;
    }
}