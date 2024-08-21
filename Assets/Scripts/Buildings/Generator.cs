using System;
using System.Collections.Generic;

public class Generator : Building, IEmployable, IWaterable, IExpensable
{
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    public int PowerProduction { get; set; }
    public int WaterConsumption { get; set; }
    public int Expenses { get; set; }
    
    public Generator(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public override void UpdateBuilding(CityData cityData)
    {
        WaterConsumption = ConsumeWater();
        PowerProduction = GeneratePower();
        Expenses = ConsumeTaxes();

        cityData.Water -= WaterConsumption;
        cityData.Power += PowerProduction;
        cityData.Earnings -= Expenses;
    }
    
    public override void UpdateBuildingStatus(CityData cityData)
    {
        IsWatered = WaterConsumption < cityData.Water;
        
        var flags = new List<bool> {IsConnectedToRoad, IsWatered};

        var hasFalse =  flags.Contains(false);

        IsActive = !hasFalse;
    }

    public int ConsumeWater()
    {
        return !IsConnectedToRoad ? 0 : 200;
    }
    
    private int GeneratePower()
    {
        return !IsActive ? 0 : 50000;
    }

    public int ConsumeTaxes()
    {
        return !IsActive ? 0 : 1000;
    }
}