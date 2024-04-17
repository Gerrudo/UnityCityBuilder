using UnityEngine;

[CreateAssetMenu(fileName = "Building Preset", menuName = "New Building Preset")]
public class BuildingPreset : ScriptableObject
{
    public string displayName;
    public int cost;
    public int population;
    public int jobs;
    public int costPerTurn;
    public int waterPerTurn;
    public int powerPerTurn;
    public GameObject prefab;
}