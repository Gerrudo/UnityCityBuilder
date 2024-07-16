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