using UnityEngine;

[CreateAssetMenu(fileName = "Building Preset", menuName = "New Building Preset")]
public class BuildingPreset : ScriptableObject
{
    public string displayName;
    public int costToBuild;
    public int taxes;
    public int expenses;
    public int population;
    public int employees;
    public int production;
        
    public GameObject prefab;
}