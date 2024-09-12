public class Road : Building
{
    public Road(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;

        Behaviours = Behaviours.None;
    }
    
    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}