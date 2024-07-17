public class WaterTower : IBuildable, IEmployable
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
        HireEmployees();
        ProduceWater();
    }
    
    public void HireEmployees()
    {
        if (Data.Employees == Data.MaxEmployees) return;

        CityData.Unemployed--;
        Data.Employees++;
    }

    public void FireEmployees()
    {
        CityData.Unemployed += Data.Employees;
    }
    
    private void ProduceWater()
    {
        CityData.Water += Data.Employees * 1000;
    }
    
    public void DestroyBuilding()
    {
        FireEmployees();
    }
}