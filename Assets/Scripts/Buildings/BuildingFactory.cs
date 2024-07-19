public interface IBuildingFactory
{
    IBuildable CreateBuilding(TileType tileType);
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