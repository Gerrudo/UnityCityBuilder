public class Commercial : IBuildable, ITaxable, IGrowable
{
    public BuildingData Data { get; set; }
    
    private City city;
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();

        Data.TileType = gameTile.TileType;
        Data.Level1TilBase = gameTile.Level1TilBase;
        
        return Data;
    }
    
    public void UpdateBuilding()
    {
        CheckBuildingLevel();
        UpdateTaxes();
        SellGoods();
    }
    
    public void UpdateTaxes()
    {
        Data.Taxes = 10;
    }
    
    public void CheckBuildingLevel()
    {
        if (Data.IsConnectedToRoad)
        {
            Data.BuildingLevel = 1;
        }
    }

    private void SellGoods()
    {
        CityData.Goods -= 5;
    }
}