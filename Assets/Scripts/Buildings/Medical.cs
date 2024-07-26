public class Medical : IBuildable, IPowerable, IWaterable
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
        ConsumePower();
        ConsumeWater();
    }

    private void SellGoods()
    {
        Data.GoodsInput = Data.Employees * 10;
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