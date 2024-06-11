using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current;

    public int day;
    public int money;
    public int bricks;
    public int population;
    public int jobs;
    public int energyProduction;
    public int energyConsumption;

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
        money -= building.moneyToBuild;
        bricks -= building.bricksToBuild;

        buildings.Add(building);
    }

    void CalculateMoney()
    {
        int buildingCostPerDay = 0;

        foreach (BuildingPreset building in buildings)
        {
            buildingCostPerDay += building.costPerDay;
        }

        money -= buildingCostPerDay;

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
            energyProduction += building.energyProduction;
            energyConsumption += building.energyConsumption;
        }
    }

    void CalculateBricks()
    {
        int brickProduction = 0;

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
        CalculateBricks();

        //I copied this from a tutorial and I hate it, surely there's a way of casting variables to UI text better than this
        //Maybe we should move all the variables out to their out text object.
        statsText.text = string.Format("Day: {0}   Money: ${1}   Bricks: {2} Tons   Energy: {3} MW/{4} MW   Workers: {5}/{6}", new object[7] { day, money,  bricks, energyProduction, energyConsumption, population, jobs });
    }
}