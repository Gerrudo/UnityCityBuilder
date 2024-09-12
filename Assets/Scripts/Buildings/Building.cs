using UnityEngine.Tilemaps;

public abstract class Building
{
    public TileType TileType;
    public TileBase TileBase;
    public bool IsConnectedToRoad;
    public bool IsPowered;
    public bool IsWatered;
    public int Population;
    public int Power;
    public int Water;
    public int Taxes;
    
    public Behaviours Behaviours;

    public abstract void Update();
}