using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current;

    public int day;
    public int bricks;
    public int maxBrickProduction;
    public int population;
    public int workers;
    public int maxWorkers;
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
        bricks -= building.bricksToBuild;

        buildings.Add(building);
    }

    void CalculateBricks()
    {
        if (workers == 0)
        {
            return;
        }

        maxBrickProduction = 0;

        foreach (BuildingPreset building in buildings)
        {
            maxBrickProduction += building.brickProduction;
        }

        bricks += maxBrickProduction * (workers / maxWorkers);
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

    void CalculateWorkers()
    {
        workers = 0;
        maxWorkers = 0;

        foreach (BuildingPreset building in buildings)
        {
            maxWorkers += building.jobs;
        }

        if (population > maxWorkers)
        {
            workers = maxWorkers;
        }
        else if (population < maxWorkers)
        {
            workers = population;
        }
    }

    void UpdateStatistics()
    {
        day++;
        CalculateEnergy();
        CalcuatePopulation();
        CalculateWorkers();
        CalculateBricks();

        //I copied this from a tutorial and I hate it, surely there's a way of casting variables to UI text better than this
        statsText.text = string.Format("Day: {0}   Bricks: {1}   Population: {2}   Energy: {3}   Workers: {4}", new object[5] { day, bricks, population, energy, workers });
    }
}