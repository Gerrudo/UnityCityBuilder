using UnityEngine.Tilemaps;

public class Building
{
    public TileType TileType;
    public TileBase TileBase;
    public bool IsConnectedToRoad;
    public bool IsActive;
    public bool IsPowered;
    public bool IsWatered;
    public int Population;
    public int Power;
    public int Water;
    public int Taxes;

    public Building(BuildingPreset buildingPreset)
    {
        TileType = buildingPreset.TileType;
        TileBase = buildingPreset.TileBase;
    }

    public void Update()
    {
        if (TileType == TileType.Generator)
        {
            Power = 100;
        }
        else
        {
            Power = -1;
        }

        Taxes = Population * 2;
        
        Population++;
    }
}