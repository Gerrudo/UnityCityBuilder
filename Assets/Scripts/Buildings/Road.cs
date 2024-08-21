public class Road : Building, IExpensable
{
    public int Expenses { get; set; }
    
    public Road(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
    }
    
    public override void UpdateBuilding(CityData cityData)
    {
        cityData.Earnings -= Expenses;
    }
    
    public override void UpdateBuildingStatus(CityData cityData)
    {
        IsActive = true;
    }
    
    public int ConsumeTaxes()
    {
        return !IsActive ? 0 : 10;
    }
}