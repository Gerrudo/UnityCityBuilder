public class Commercial : IBuildable, IGrowable, IEmployable, IPowerable, IWaterable
{
    public BuildingData Data { get; set; }
    
    private City city;
    
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
        SellGoods();
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

    private void SellGoods()
    {
        if (CityData.Goods <= 0) return;

        CityData.Goods -= Data.Employees * 10;
        CityData.Earnings += 10;
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