public class Service : Building
{
    public Service(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}