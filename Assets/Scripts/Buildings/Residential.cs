using System.Collections.Generic;

public class Residential : IBuildable, ITaxable, IGrowable, IPowerable, IWaterable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        
        Data.TileType = gameTile.TileType;
        Data.Level1TilBase = gameTile.Level1TilBase;
        Data.MaxPopulation = gameTile.MaxPopulation;
        
        Data.Residents = new List<Citizen>();
        
        return Data;
    }
    
    public void UpdateBuilding()
    {
        CheckBuildingLevel();
        UpdatePopulation();
        ConsumePower();
        ConsumeWater();
        UpdateTaxes();
    }

    private void UpdatePopulation()
    {
        Data.CurrentPopulation = Data.Residents.Count;

        Data.Unemployed = 0;

        foreach (var resident in Data.Residents)
        {
            if (!resident.IsEmployed) continue;

            Data.Unemployed++;
        }
    }

    public void UpdateTaxes()
    {
        Data.Taxes = Data.CurrentPopulation * 2;
    }

    public void CheckBuildingLevel()
    {
        if (Data.IsConnectedToRoad)
        {
            Data.BuildingLevel = 1;
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
    
    public void DestroyBuilding()
    {
        Data.Unemployed -= Data.CurrentPopulation;
    }
}