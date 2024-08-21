using System;
using System.Collections.Generic;

public class WaterTower : Building, IEmployable, IPowerable, IExpensable
{
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    public int WaterProduction { get; set; }
    public int PowerConsumption { get; set; }
    public int Expenses { get; set; }
    
    public WaterTower(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public override void UpdateBuilding(CityData cityData)
    {
        PowerConsumption = ConsumePower();
        WaterProduction = GenerateWater();
        Expenses = ConsumeTaxes();

        cityData.Power -= PowerConsumption;
        cityData.Water += WaterProduction;
        cityData.Earnings -= Expenses;
    }
    
    public override void UpdateBuildingStatus(CityData cityData)
    {
        var flags = new List<bool> {IsConnectedToRoad};

        var hasFalse =  flags.Contains(false);

        IsActive = !hasFalse;
    }
    
    public int GenerateWater()
    {
        return !IsActive ? 0 : 75000;
    }

    public int ConsumePower()
    {
        return !IsConnectedToRoad ? 0 : 200;
    }
    
    public int ConsumeTaxes()
    {
        return !IsActive ? 0 : 1000;
    }
}