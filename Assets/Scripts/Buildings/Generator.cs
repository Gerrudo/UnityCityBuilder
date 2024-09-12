public class Generator : Building
{
    public Generator(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}