public class WaterTower : IBuildable, IEmployable, IPowerable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        Data.TileType = gameTile.TileType;
        Data.Expenses = gameTile.Expenses;
        Data.MaxEmployees = gameTile.MaxEmployees;
        Data.WaterProduction = 250000;
        
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
        Data.PowerConsumption = Data.Employees * 4;
    }
    
    public void DestroyBuilding()
    {
        FireEmployees();
    }
}