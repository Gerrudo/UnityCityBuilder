public class Commercial : IBuildable, ITaxable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        Data.TileType = gameTile.TileType;
        
        return Data;
    }
    
    public void UpdateBuilding()
    {
        UpdateTaxes();
    }
    
    public void UpdateTaxes()
    {
        Data.Taxes = 10;
    }
}