public class Extractor : IBuildable
{
    public BuildingData Data { get; set; }
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateBuilding()
    {
        throw new System.NotImplementedException();
    }

    public void DestroyBuilding()
    {
        throw new System.NotImplementedException();
    }
}