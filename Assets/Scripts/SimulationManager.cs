using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current;

    public int day;

    public int money;
    public int income;
    public int expenses;

    public int clay;
    public int clayConsumption;
    public int clayProduction;
    public int bricks;
    public int brickProduction;

    public int population;
    public int jobs;

    public int coal;
    public int coalConsumption;
    public int coalProduction;
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
        InvokeRepeating("UpdateStatistics", 0.0f, 2.0f);
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
            income += building.taxesPerDay;
            expenses += building.costPerDay;
        }

        money += income;
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

    void CalculateCoal()
    {
        coalConsumption = 0;
        coalProduction = 0;

        foreach (BuildingPreset building in buildings)
        {
            coalConsumption += building.coalConsumption;
            coalProduction += building.coalProduction;
        }
    }

    void CalculateClay()
    {
        clayConsumption = 0;
        clayProduction = 0;

        foreach (BuildingPreset building in buildings)
        {
            clayConsumption += building.clayConsumption;
            clayProduction += building.clayProduction;
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

        if (coalProduction < coalConsumption)
        {
            energyProduction = 0;

            string statusMessage = "Not enough coal.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return;
        }
    }

    void CalculateBricks()
    {
        brickProduction = 0;

        if (CanProduce())
        {
            foreach (BuildingPreset building in buildings)
            {
                brickProduction += building.brickProduction;
            }
        }

        bricks += brickProduction;
    }

    private bool CanProduce()
    {
        if (energyProduction < energyConsumption)
        {
            string statusMessage = "Not enough power.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return false;
        }

        if (population < jobs)
        {
            string statusMessage = "Not enough workers.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return false;
        }

        if (waterProduction < waterConsumption)
        {
            string statusMessage = "Not enough water.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return false;
        }

        if (clayProduction < clayConsumption)
        {
            string statusMessage = "Not enough clay.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return false;
        }

        return true;
    }

    void UpdateStatistics()
    {
        day++;
        CalculateCoal();
        CalculateClay();
        CalculateMoney();
        CalcuatePopulation();
        CalculateJobs();
        CalculateEnergy();
        CalculateWater();
        CalculateBricks();

        UIManager.current.UpdateStatsUI();
    }
}