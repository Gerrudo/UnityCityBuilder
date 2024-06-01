using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current;

    public int day;
    public int bricks;
    public int population;
    public int workers;
    public int maxWorkers;
    public int energy;
    public int maxEnergyProduction;
    public int maxEnergyConsumption;
    public decimal totalBrickModifier;

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

    void CalculateModifiers()
    {
        decimal totalBrickModifier = 1;

        List<decimal> modiifiers = new List<decimal>();

        //To not divide by zero.
        if (workers > 0)
        {
            decimal workerModifier = (decimal)workers / (decimal)maxWorkers;

            modiifiers.Add(workerModifier);
        }

        //To not divide by zero.
        if (maxEnergyProduction > 0)
        {
            decimal energyModifer = (decimal)maxEnergyProduction / (decimal)maxEnergyConsumption;

            modiifiers.Add(energyModifer);
        }

        foreach (var modiifier in modiifiers)
        {
            //The modifier cannot be greater than 100% production rate.
            if (modiifier < 1)
            {
                totalBrickModifier += modiifier / modiifiers.Count;
            }
            else
            {
                totalBrickModifier += 1 / modiifiers.Count;
            }
        }
    }

    void CalculateBricks()
    {
        int maxBrickProduction = 0;

        foreach (BuildingPreset building in buildings)
        {
            maxBrickProduction += building.brickProduction;
        }

        bricks += maxBrickProduction * totalBrickModifier;
    }

    void UpdateStatistics()
    {
        day++;
        CalcuatePopulation();
        CalculateWorkers();
        CalculateEnergy();
        CalculateModifiers();
        CalculateBricks();

        //I copied this from a tutorial and I hate it, surely there's a way of casting variables to UI text better than this
        statsText.text = string.Format("Day: {0}   Bricks: {1}   Population: {2}   Energy: {3}   Workers: {4}    Production Rate: {5}", new object[6] { day, bricks, population, energy, workers, totalBrickModifier });
    }
}