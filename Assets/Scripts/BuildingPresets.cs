using UnityEngine;

[CreateAssetMenu(fileName = "Building Preset", menuName = "New Building Preset")]
public class BuildingPreset : ScriptableObject
{
    public string displayName;

    public enum BuildingType
    {
        Residential,
        Commerical,
        Industrial,
        Generator,
        Water,
        Road,
        Mine,
        Quarry
    }

    public BuildingType buildingType;
    public int moneyToBuild;
    public int bricksToBuild;
    public GameObject prefab;
}