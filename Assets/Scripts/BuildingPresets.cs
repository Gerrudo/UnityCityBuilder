using UnityEngine;

[CreateAssetMenu(fileName = "Building Preset", menuName = "New Building Preset")]
public class BuildingPreset : ScriptableObject
{
    public string displayName;
    public int bricksToBuild;
    public int population;
    public int jobs;
    public int energyConsumption;
    public int energyProduction;
    public int brickProduction;
        
    public GameObject prefab;
}