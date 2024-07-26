public class Extractor : IBuildable
{
    public BuildingData Data { get; set; }
    public BuildingData NewBuildingData(Preset buildingPreset)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateBuilding()
    {
        throw new System.NotImplementedException();
    }
}