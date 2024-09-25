public class BuildingFactory
{
    public Building CreateBuilding(BuildingPreset buildingPreset)
    {
        return buildingPreset.TileType switch
        {
            TileType.Road => new Road(buildingPreset),
            TileType.Residential => new Residential(buildingPreset),
            TileType.Commercial => new Commercial(buildingPreset),
            TileType.Industrial => new Industrial(buildingPreset),
            TileType.Generator => new Generator(buildingPreset),
            TileType.WaterTower => new WaterPump(buildingPreset),
            TileType.Medical or TileType.Fire or TileType.Police => new Service(buildingPreset),
            _ => throw new System.Exception("No Preset was provided.")
        };
    }
}