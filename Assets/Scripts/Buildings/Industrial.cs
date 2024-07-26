public class Industrial : IBuildable, IGrowable, IPowerable, IWaterable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(Preset buildingPreset)
    {
        Data = new BuildingData();

        Data.TileType = buildingPreset.TileType;
        Data.Level1TilBase = buildingPreset.Level1TilBase;
        Data.MaxEmployees = buildingPreset.MaxEmployees;
        
        return Data;
    }
    
    public void UpdateBuilding()
    {
        CheckBuildingLevel();
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
}