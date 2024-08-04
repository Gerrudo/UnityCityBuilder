using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Commercial : Building, IEmployer, IGrowable, ITaxable, IPower, IWater
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public TileBase Level1TilBase { get; set; }

    public int MaxEmployees { get; set; }
    public List<Guid> Jobs { get; set; }
    
    public Commercial(Preset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        Level1TilBase = buildingPreset.Level1TilBase;
        MaxEmployees = buildingPreset.MaxEmployees;
        Jobs = new List<Guid>();
    }
    
    public void CanUpgrade()
    {
        if (IsConnectedToRoad && Jobs.Count > 10)
        {
            TileBase = Level1TilBase;
        }
    }
    
    public int GenerateWater()
    {
        return 0;
    }

    public int ConsumeWater()
    {
        return Jobs.Count * 4;
    }
    
    public int GeneratePower()
    {
        return 0;
    }

    public int ConsumePower()
    {
        return Jobs.Count * 4;
    }
    
    public int CalculateTaxes()
    {
        return Jobs.Count * 5;
    }
}