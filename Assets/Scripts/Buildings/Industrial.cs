public class Industrial : IBuildable, IGrowable, IEmployable, IPowerable, IWaterable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();

        Data.TileType = gameTile.TileType;
        Data.Level1TilBase = gameTile.Level1TilBase;
        Data.MaxEmployees = gameTile.MaxEmployees;
        
        return Data;
    }
    
    public void UpdateBuilding()
    {
        CheckBuildingLevel();
        HireEmployees();
        ConsumePower();
        ConsumeWater();
        ProduceGoods();
    }
    
    public void CheckBuildingLevel()
    {
        if (Data.IsConnectedToRoad)
        {
            Data.BuildingLevel = 1;
        }
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
    
    private void ProduceGoods()
    {
        CityData.Goods += Data.Employees * 10;
        CityData.Earnings += 5;
    }
    
    public void ConsumePower()
    {
        CityData.Power -= Data.Employees * 4;
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