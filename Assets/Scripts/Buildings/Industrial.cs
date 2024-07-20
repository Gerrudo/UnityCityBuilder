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
        if (Data.Employees >= Data.MaxEmployees) return;

        Data.Unemployed -= 2;
        Data.Employees += 2;
    }

    public void FireEmployees()
    {
        Data.Unemployed -= Data.Employees;
    }
    
    private void ProduceGoods()
    {
        Data.GoodsOutput = Data.Employees * 10;
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