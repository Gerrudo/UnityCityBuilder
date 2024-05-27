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
    public int maxPopulation;
    public int maxEmployment;

    public TextMeshProUGUI statsText;

    private List<BuildingPreset> buildings = new List<BuildingPreset>();

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //Repeating starts instantly, repeats every second.
        InvokeRepeating("UpdateStatistics", 0.0f, 1.0f);
    }

    public void OnPlaceBuilding(BuildingPreset building)
    {
        if (bricks > building.costToBuild)
        {
            //Building has maximum values which need to be handled when seeing how much the city can grow.
            maxPopulation += building.population;
            maxEmployment += building.employees;

            bricks -= building.costToBuild;

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

    void CalcuatePopulation()
    {
        //I don't like doing this, must be better way. Instead check if anything has changed and process, rather than resetting the value.
        maxPopulation = 0;

        foreach (BuildingPreset building in buildings)
        {
            maxPopulation += building.population;
        }
    }

    void CalculateEmployment()
    {
        //Don't like, same as maxPopulation being zero'd
        employment = 0;
        maxEmployment = 0;

        foreach (BuildingPreset building in buildings)
        {
            maxEmployment += building.employees;
        }

        //Employment cannot be higher than population.
        employment = Mathf.Min(population, maxEmployment);
    }

    void UpdateStatistics()
    {
        day++;
        CalculateBricks();
        CalcuatePopulation();
        CalculateEmployment();

        //I copied this from a tutorial and I hate it, surely there's a way of casting variables to UI text better than this
        statsText.text = string.Format("Day: {0}   Bricks: {1}   Population: {2} / {3}   Employment: {4} / {5}", new object[7] { day, bricks, population, maxPopulation, employment, maxEmployment, consumables });
    }
}