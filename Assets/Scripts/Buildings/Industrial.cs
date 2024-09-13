public class Industrial : Building
{
    public Industrial(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;

        Jobs = 50;
    }
    public override void Update()
    {
        Earnings = !IsConnectedToRoad ? 100 : 0;
    }
}