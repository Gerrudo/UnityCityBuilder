public class Generator : IBuildable, IWaterable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(Preset buildingPreset)
    {
        Data = new BuildingData();
        Data.TileType = buildingPreset.TileType;
        Data.Expenses = buildingPreset.Expenses;
        Data.MaxEmployees = buildingPreset.MaxEmployees;
        Data.PowerOutput = 50000;
        
        return Data;
    }
    
    public void UpdateBuilding()
    {
        ConsumeWater();
    }

    public void ConsumeWater()
    {
        Data.WaterInput = Data.Employees * 2;
    }
}