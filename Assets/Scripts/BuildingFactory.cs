using UnityEngine.Tilemaps;

public interface IBuildable
{
    BuildingData Data
    {
        get;
        set;
    }
    BuildingData NewBuildingData(GameTile gameTile);
    void UpdateBuilding();
}

public interface ITaxable
{
    void UpdateTaxes();
}

public interface IBuildingFactory
{
    IBuildable CreateBuilding(TileType tileType);
}

public class BuildingData
{
    public TileType TileType { get; set; }
    public TileBase Level1TilBase { get; set; }
    public int CurrentPopulation { get; set; }
    public int MaxPopulation { get; set; }
    public int Taxes { get; set; }
    public int Expenses { get; set; }
}

public class BuildingFactory : IBuildingFactory
{
    public IBuildable CreateBuilding(TileType tileType)
    {
        return tileType switch
        {
            TileType.Road => new Road(),
            TileType.Residential => new Residential(),
            TileType.Commercial => new Commercial(),
            TileType.Industrial => new Industrial(),
            TileType.Generator => new Generator(),
            TileType.WaterTower => new WaterTower(),
            _ => throw new System.Exception("No TileType was provided.")
        };
    }
}