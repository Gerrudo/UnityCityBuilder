public class Police : IBuildable, IPowerable, IWaterable, IEmployable
{
    public BuildingData Data { get; set; }
    public BuildingData NewBuildingData(Preset buildingPreset)
    {
        Data = new BuildingData();

        Data.TileType = buildingPreset.TileType;
        Data.MaxEmployees = buildingPreset.MaxEmployees;
        
        return Data;
    }

    public void UpdateBuilding()
    {
        HireEmployees();
        ConsumePower();
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
    
    public void ConsumePower()
    {
        Data.PowerInput = Data.Employees * 4;
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