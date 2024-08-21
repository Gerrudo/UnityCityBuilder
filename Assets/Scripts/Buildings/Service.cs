using System;
using System.Collections.Generic;

public class Service : Building, IEmployable, IWaterable, IPowerable, IExpensable
{
    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    public int PowerConsumption { get; set; }
    public int WaterConsumption { get; set; }
    public int Expenses { get; set; }
    
    public Service(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }

    public override void UpdateBuilding(CityData cityData)
    {
        WaterConsumption = ConsumeWater();
        PowerConsumption = ConsumePower();
        Expenses = ConsumeTaxes();

        cityData.Water -= WaterConsumption;
        cityData.Power += PowerConsumption;
        cityData.Earnings -= Expenses;
    }
    
    public override void UpdateBuildingStatus(CityData cityData)
    {
        IsWatered = WaterConsumption < cityData.Water;
        IsPowered = PowerConsumption < cityData.Power;
        
        var flags = new List<bool> {IsConnectedToRoad, IsPowered, IsWatered};

        var hasFalse =  flags.Contains(false);

        IsActive = !hasFalse;
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

    public int ConsumeTaxes()
    {
        return !IsActive ? 0 : 1000;
    }
}