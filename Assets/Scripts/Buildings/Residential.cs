public class Residential : IBuildable, ITaxable, IGrowable, IPowerable, IWaterable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        
        Data.TileType = gameTile.TileType;
        Data.Level1TilBase = gameTile.Level1TilBase;
        Data.MaxPopulation = gameTile.MaxPopulation;
        
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
        if (Data.CurrentPopulation < Data.MaxPopulation && Data.IsConnectedToRoad)
        {
            Data.CurrentPopulation++;
            CityData.Unemployed++;
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
        CityData.Power -= Data.CurrentPopulation * 4;
    }

    public void ConsumeWater()
    {
        CityData.Water -= Data.CurrentPopulation * 2;
    }
    
    public void DestroyBuilding()
    {
        CityData.Unemployed -= Data.CurrentPopulation;
    }
}