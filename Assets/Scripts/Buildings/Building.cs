using UnityEngine.Tilemaps;

public abstract class Building
{
    public TileType TileType;
    public TileBase TileBase;
    public bool IsConnectedToRoad;
    public bool IsActive;
    public bool IsPowered;
    public bool IsWatered;

    public abstract void UpdateBuildingStatus(CityData cityData);
    public abstract void UpdateBuilding(CityData cityData);
}