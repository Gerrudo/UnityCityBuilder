public interface IBuildable
{
    BuildingData Data
    {
        get;
        set;
    }
    BuildingData NewBuildingData(Preset buildingPreset);
    void UpdateBuilding();
}