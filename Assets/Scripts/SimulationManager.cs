using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current;

    public int day;

    public int money;
    public int income;
    public int expenses;

    public int bricks;
    public int brickProduction;

    public int population;
    public int jobs;

    public int energyConsumption;
    public int energyProduction;

    public int waterConsumption;
    public int waterProduction;

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
        money -= building.moneyToBuild;
        bricks -= building.bricksToBuild;

        buildings.Add(building);
    }

    void CalculateMoney()
    {
        //Income is not yet being used
        income = 0;
        expenses = 0;

        foreach (BuildingPreset building in buildings)
        {
            expenses += building.costPerDay;
        }

        money -= expenses;

        if (money <= 0)
        {
            UIManager.current.StartGameOver();
        }
    }

    void CalcuatePopulation()
    {
        population = 0;

        foreach (BuildingPreset building in buildings)
        {
            population += building.population;
        }
    }

    void CalculateJobs()
    {
        //workers is not accounted for, as it is the total population.
        jobs = 0;

        foreach (BuildingPreset building in buildings)
        {
            jobs += building.jobs;
        }
    }

    void CalculateEnergy()
    {
        energyProduction = 0;
        energyConsumption = 0;

        foreach (BuildingPreset building in buildings)
        {
            energyConsumption += building.energyConsumption;
            energyProduction += building.energyProduction;
        }
    }

    void CalculateWater()
    {
        waterProduction = 0;
        waterConsumption = 0;

        foreach (BuildingPreset building in buildings)
        {
            waterConsumption += building.waterConsumption;
            waterProduction += building.waterProduction;
        }
    }

    void CalculateBricks()
    {
        brickProduction = 0;

        if (energyProduction < energyConsumption)
        {
            string statusMessage = "Not enough power to produce bricks.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return;
        }

        if (population < jobs)
        {
            string statusMessage = "Not enough workers to produce bricks.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return;
        }

        if (waterProduction < waterConsumption)
        {
            string statusMessage = "Not enough water to produce bricks.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return;
        }

        foreach (BuildingPreset building in buildings)
        {
            brickProduction += building.brickProduction;
        }

        bricks += brickProduction;
    }

    void UpdateStatistics()
    {
        day++;
        CalculateMoney();
        CalcuatePopulation();
        CalculateJobs();
        CalculateEnergy();
        CalculateWater();
        CalculateBricks();

        UIManager.current.UpdateStatsUI();
    }
}