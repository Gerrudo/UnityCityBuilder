public class Road : IBuildable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        
        Data.TileType = gameTile.TileType;
        Data.Expenses = gameTile.Expenses;
        
        return Data;
    }

    public void UpdateBuilding()
    {
        
    }
    
    public void DestroyBuilding()
    {
        
    }
}