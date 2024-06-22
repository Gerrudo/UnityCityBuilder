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
    public int costPerDay;
    public int taxesPerDay;

    public int brickProduction;
    public int bricksToBuild;

    public int population;
    public int jobs;

    public int energyConsumption;
    public int energyProduction;

    public int waterProduction;
    public int waterConsumption;

    public int clayConsumption;
    public int clayProduction;

    public int coalConsumption;
    public int coalProduction;

    public GameObject prefab;
}