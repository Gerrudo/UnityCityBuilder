public class Generator : Building
{
    public Generator(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
    }
    public override void Update()
    {
        Earnings = -100;
        Power = !IsConnectedToRoad ? 100 : 0;
    }
}