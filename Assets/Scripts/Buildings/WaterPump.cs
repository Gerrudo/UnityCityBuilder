public class WaterPump : Building
{
    public WaterPump(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
    }
    public override void Update()
    {
        if (!IsConnectedToRoad) return;
        
        Earnings = -50;
        Water = 250;
    }
}