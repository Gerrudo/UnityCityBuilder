public class Generator : IBuildable, IEmployable, IWaterable
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
        HireEmployees();
        ConsumeWater();
    }
    
    public void HireEmployees()
    {
        if (Data.Employees >= Data.MaxEmployees) return;

        Data.Unemployed -= 2;
        Data.Employees += 2;
    }

    public void FireEmployees()
    {
        Data.Unemployed -= Data.Employees;
    }

    public void ConsumeWater()
    {
        Data.WaterInput = Data.Employees * 2;
    }
    
    public void DestroyBuilding()
    {
        FireEmployees();
    }
}