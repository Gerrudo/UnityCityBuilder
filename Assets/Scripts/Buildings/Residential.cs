using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Residential : Building, IGrowable, IResidence, IWater, IEarnings, IApproval
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
    
    private int GetEmploymentScore()
    {
        return 0;
    }
    
    private int GetServiceScore(TileType tileType)
    {
        return 0;
    }
}