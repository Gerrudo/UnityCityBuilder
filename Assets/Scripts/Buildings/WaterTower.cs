public class WaterTower : IBuildable, IPowerable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(Preset buildingPreset)
    {
        Data = new BuildingData();
        Data.TileType = buildingPreset.TileType;
        Data.Expenses = buildingPreset.Expenses;
        Data.MaxEmployees = buildingPreset.MaxEmployees;
        Data.WaterOutput = 25000;
        
        return Data;
    }
    
    public void UpdateBuilding()
    {
        ConsumePower();
    }
    
    public void ConsumePower()
    {
        Data.PowerInput = Data.Employees * 4;
    }
}