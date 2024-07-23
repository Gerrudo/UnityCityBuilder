public class WaterTower : IBuildable, IEmployable, IPowerable
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
        HireEmployees();
        ConsumePower();
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
    
    public void ConsumePower()
    {
        Data.PowerInput = Data.Employees * 4;
    }
    
    public void DestroyBuilding()
    {
        FireEmployees();
    }
}