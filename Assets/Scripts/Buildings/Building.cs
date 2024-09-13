using UnityEngine.Tilemaps;
using System.Threading;

public abstract class Building
{
    protected Timer Tick;
    
    public TileType TileType;
    public TileBase TileBase;
    public bool IsConnectedToRoad;
    public int Power;
    public int Water;
    public int Earnings;
    public int Population;
    public int Jobs;
    public int UpdateTick = 1000;

    public abstract void Update();
}