public class Road : IBuildable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(Preset buildingPreset)
    {
        Data = new BuildingData();
        
        Data.TileType = buildingPreset.TileType;
        Data.Expenses = buildingPreset.Expenses;
        
        return Data;
    }

    public void UpdateBuilding()
    {
        
    }
    
    public void DestroyBuilding()
    {
        
    }
}