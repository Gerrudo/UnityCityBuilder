public interface ICalculable
{
    PlaceableTile Calculate(PlaceableTile tile);
}

public class Placeable : ICalculable
{
    public PlaceableTile Calculate(PlaceableTile tile)
    {
        switch (tile.TileType)
        {
            case TileType.Residential:
                tile.CurrentPopulation = Calculations.Population(tile.MaxPopulation, tile.CurrentPopulation);
                tile.WaterDemand = Calculations.Water(tile.CurrentPopulation);
                tile.PowerDemand = Calculations.Power(tile.CurrentPopulation);
                tile.Taxes = Calculations.Income(tile.CurrentPopulation);
                break;
            case TileType.Commercial:
                tile.WaterDemand = Calculations.Water(tile.CurrentPopulation);
                tile.PowerDemand = Calculations.Power(tile.CurrentPopulation);
                tile.Taxes = Calculations.Income(tile.CurrentPopulation);
                break;
            case TileType.Industrial:
                tile.WaterDemand = Calculations.Water(tile.CurrentPopulation);
                tile.PowerDemand = Calculations.Power(tile.CurrentPopulation);
                tile.Taxes = Calculations.Income(tile.CurrentPopulation);
                break;
        }

        return tile;
    }
}