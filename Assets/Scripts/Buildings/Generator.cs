public class Generator : IBuildable, IEmployable, IWaterable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        Data.TileType = gameTile.TileType;
        Data.Expenses = gameTile.Expenses;
        Data.MaxEmployees = gameTile.MaxEmployees;
        Data.PowerOutput = 500000;
        
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