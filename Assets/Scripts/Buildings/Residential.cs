using System.Linq;
using System.Collections.Generic;

public class Residential : IBuildable, ITaxable, IGrowable, IPowerable, IWaterable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(Preset buildingPreset)
    {
        Data = new BuildingData();
        
        Data.TileType = buildingPreset.TileType;
        Data.Level1TilBase = buildingPreset.Level1TilBase;
        Data.MaxPopulation = buildingPreset.MaxPopulation;
        Data.TileBase = buildingPreset.TileBase;
        
        Data.Residents = new List<Citizen>();
        
        return Data;
    }
    
    public void UpdateBuilding()
    {
        UpgradeBuilding();
        CountPopulation();
        CountUnemployed();
        ConsumePower();
        ConsumeWater();
        UpdateTaxes();
    }

    private void CountPopulation()
    {
        Data.CurrentPopulation = Data.Residents.Count;
    }

    private void CountUnemployed()
    {
        Data.Unemployed = Data.Residents.Count(citizen => !citizen.IsEmployed);
    }

    public void UpdateTaxes()
    {
        Data.Taxes = Data.CurrentPopulation * 2;
    }

    public void UpgradeBuilding()
    {
        if (Data.IsConnectedToRoad && Data.CurrentPopulation > 10)
        {
            Data.TileBase = Data.Level1TilBase;
        }
    }
    
    public void ConsumePower()
    {
        Data.PowerInput = Data.CurrentPopulation * 4;
    }

    public void ConsumeWater()
    {
        Data.WaterInput = Data.CurrentPopulation * 2;
    }
}