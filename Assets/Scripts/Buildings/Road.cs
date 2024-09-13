public class Road : Building
{
    public Road(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        
        Earnings = -10;
    }
    
    public override void Update()
    {
        
    }
}