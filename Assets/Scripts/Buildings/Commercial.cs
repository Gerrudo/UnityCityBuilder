public class Commercial : Building
{
    public Commercial(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        
        Jobs = 25;
    }
    public override void Update()
    {
        Earnings = !IsConnectedToRoad ? 100 : 0;
    }
}