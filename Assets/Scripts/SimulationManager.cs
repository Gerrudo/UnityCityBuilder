using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current;

    private int day;

    public int money;

    public int bricks;

    private int population;
    private int jobs;

    private int energyProduction;
    private int energyConsumption;

    private int waterProduction;
    private int waterConsumption;

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

    void CalculateWater()
    {
        waterProduction = 0;
        waterConsumption = 0;

        foreach (BuildingPreset building in buildings)
        {
            waterProduction += building.waterProduction;
            waterConsumption += building.waterConsumption;
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

        //I copied this from a tutorial and I hate it, surely there's a way of casting variables to UI text better than this
        //Maybe we should move all the variables out to their out text object.
        statsText.text = string.Format("Day: {0}   Money: ${1}   Bricks: {2} Tons   Energy: {3} MW/{4} MW   Water: {5} Kl/{6} Kl   Workers: {7}/{8}", new object[9] { day, money,  bricks, energyConsumption, energyProduction, waterConsumption, waterProduction, population, jobs });
    }
}