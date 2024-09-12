using UnityEngine.Tilemaps;

public abstract class Building
{
    public TileType TileType;
    public TileBase TileBase;
    public bool IsConnectedToRoad;
    public int Power;
    public bool Powered;
    public int Water;
    public bool Watered;
    public int Earnings;
    public int Population;

    public abstract void Update();
}