public class Generator : IBuildable, IEmployable, IWaterable
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
        ConsumeWater();
        ProducePower();
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

    private void ProducePower()
    {
        CityData.Power += Data.Employees * 100;
    }

    public void ConsumeWater()
    {
        CityData.Water -= Data.Employees * 2;
    }
    
    public void DestroyBuilding()
    {
        FireEmployees();
    }
}