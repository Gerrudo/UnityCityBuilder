public class Road : Building
{
    public Road(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        
        Earnings = -100;
    }
    
    public override void Update()
    {
        
    }
}