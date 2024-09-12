using System;

public class Generator : Building
{
    public Generator(BuildingPreset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        
        Earnings = -100;
        Power = 100;
    }
    public override void Update()
    {
        
    }
}