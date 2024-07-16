public class Residential : IBuildable, ITaxable, IGrowable
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
        UpdateTaxes();
    }

    private void UpdatePopulation()
    {
        if (Data.CurrentPopulation < Data.MaxPopulation && Data.IsConnectedToRoad)
        {
            Data.CurrentPopulation++;
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
}