public interface ICalculable
{
    PlaceableTile Calculate(PlaceableTile tile);
}

public class Placeable : ICalculable
{
    public PlaceableTile Calculate(PlaceableTile tile)
    {
        PlaceableTile newTile = tile;

        switch (tile.TileType)
        {
            case TileType.Residential:
                tile.CurrentPopulation = Calculations.Population(tile.MaxPopulation, tile.Happiness, tile.CurrentPopulation);
                tile.WaterDemand = Calculations.Water(tile.CurrentPopulation);

                break;
            case TileType.Commerical:

                break;
            case TileType.Industrial:

                break;
            case TileType.Generator:

                break;
            case TileType.WaterTower:

                break;
            default:
                break;
        }

        return newTile;
    }
}