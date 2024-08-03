using UnityEngine.Tilemaps;

public abstract class Building
{
    public abstract TileType TileType { get; set; }
    public abstract TileBase TileBase { get; set; }
    public abstract bool IsConnectedToRoad { get; set; }
}