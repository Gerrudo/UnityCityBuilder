using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Residential : Building, IGrowable, IResidence
{
    public sealed override TileType TileType { get; set; }
    public sealed override TileBase TileBase { get; set; }
    public override bool IsConnectedToRoad { get; set; }
    public int MaxPopulation { get; set; }
    public List<Citizen> Residents { get; set; }
    public TileBase Level1TilBase { get; set; }
    
    public Residential(Preset buildingPreset)
    {
        TileBase = buildingPreset.TileBase;
        TileType = buildingPreset.TileType;
        Level1TilBase = buildingPreset.Level1TilBase;
        MaxPopulation = buildingPreset.MaxPopulation;
        
        Residents = new List<Citizen>();
    }

    public void CanUpgrade()
    {
        if (IsConnectedToRoad && Residents.Count > 10)
        {
            TileBase = Level1TilBase;
        }
    }
}