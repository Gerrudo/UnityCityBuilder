public class BuildingFactory : IBuildingFactory
{
    public Building CreateBuilding(Preset buildingPreset)
    {
        return buildingPreset.TileType switch
        {
            TileType.Road => new Road(buildingPreset),
            TileType.Residential => new Residential(buildingPreset),
            TileType.Commercial => new Commercial(buildingPreset),
            TileType.Industrial => new Industrial(buildingPreset),
            TileType.Generator => new Generator(buildingPreset),
            TileType.WaterTower => new WaterTower(buildingPreset),
            TileType.Medical => new Hospital(buildingPreset),
            TileType.Fire => new FireStation(buildingPreset),
            TileType.Police => new PoliceStation(buildingPreset),
            _ => throw new System.Exception("No Preset was provided.")
        };
    }
}