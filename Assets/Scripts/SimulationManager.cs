using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager current;

    public int treasury;
    public int population;
    public int employment;
    public int consumables;
    public int maxPopulation;
    public int maxEmployment;
    public int tax;

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
        //Building has maximum values which need to be handled when seeing how much the city can grow.
        maxPopulation += building.population;
        maxEmployment += building.employees;

        buildings.Add(building);
    }

    void CalculateIncome()
    {
        //Collect taxes from each RCI building.
        treasury += employment * tax;

        foreach (BuildingPreset building in buildings)
        {
            //Expenses to manage, these need to be handled elserwhere and totalled up to here.
            //Expenses will need to be expanded and not managed on a perbuilding level, as buildings will use different amounts of several resources.
            treasury -= building.expenses;
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

        //if consumables are greater than the population and population is less than the maximum, increase population.
        if (consumables >= population && population < maxPopulation)
        {
            //Each population requires 4 consumables
            consumables -= population / 4;
            //Yeah I copied this. Will find whichever is smaller: the population + the amount of consumables divided by 4, or the current population. Probably.
            population = Mathf.Min(population + (consumables / 4), population);
        }
        else if (consumables < population)
        {
            population = consumables;
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

    void CalculateConsumables()
    {
        //xD
        consumables = 0;

        foreach (BuildingPreset building in buildings)
        {
            consumables += building.production;
        }
    }

    void UpdateStatistics()
    {
        CalculateIncome();
        CalcuatePopulation();
        CalculateEmployment();
        CalculateConsumables();
    }
}