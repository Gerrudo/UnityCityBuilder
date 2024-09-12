public class Residential : Building
{
    public Residential(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
    }
    
    public override void Update()
    {
        if (!IsConnectedToRoad) return;
        if (Population >= 100) return;
        
        Population++;
        Earnings = Population * 2;
    }
}