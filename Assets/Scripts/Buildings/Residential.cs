using System.Threading;

public class Residential : Building
{
    public Residential(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        
        base.Tick = new Timer(Tick, null, 0, UpdateTick);
    }
    
    public override void Update()
    {

    }

    private new void Tick(object state)
    {
        if (!IsConnectedToRoad) return;
        if (Population >= 100) return;
        
        Population++;
        Earnings = Population * 2;
    }
}