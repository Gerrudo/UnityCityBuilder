using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current;

    public int day;
    public int bricks;
    public int population;
    public int employment;
    public int energy;
    public int maxEnergyProduction;
    public int maxEnergyConsumption;

    public TextMeshProUGUI statsText;

    private List<BuildingPreset> buildings = new List<BuildingPreset>();

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        InvokeRepeating("UpdateStatistics", 0.0f, 1.0f);
    }

    public void OnPlaceBuilding(BuildingPreset building)
    {
        if (bricks > building.bricksToBuild)
        {
            bricks -= building.bricksToBuild;

            buildings.Add(building);
        }
        else
        {
            Debug.Log("Not enough bricks to place building.");
        }
    }

    void CalculateBricks()
    {
        foreach (BuildingPreset building in buildings)
        {
            bricks += building.brickProduction;
        }
    }

    void CalculateEnergy()
    {
        energy = 0;
        maxEnergyProduction = 0;
        maxEnergyConsumption = 0;

        foreach (BuildingPreset building in buildings)
        {
            maxEnergyProduction += building.energyProduction;
            maxEnergyConsumption += building.energyConsumption;
        }

        energy = (maxEnergyProduction - maxEnergyConsumption);
    }

    void CalcuatePopulation()
    {
        population = 0;

        foreach (BuildingPreset building in buildings)
        {
            population += building.population;
        }
    }

    void UpdateStatistics()
    {
        day++;
        CalculateBricks();
        CalculateEnergy();
        CalcuatePopulation();

        //I copied this from a tutorial and I hate it, surely there's a way of casting variables to UI text better than this
        statsText.text = string.Format("Day: {0}   Bricks: {1}   Population: {2}   Energy: {3}", new object[4] { day, bricks, population, energy });
    }
}