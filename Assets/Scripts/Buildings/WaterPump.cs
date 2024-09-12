public class WaterPump : Building
{
    public WaterPump(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}