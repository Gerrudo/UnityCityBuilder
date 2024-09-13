public class WaterPump : Building
{
    public WaterPump(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
    }
    public override void Update()
    {
        Earnings = -50;
        Water = !IsConnectedToRoad ? 250 : 0;
    }
}