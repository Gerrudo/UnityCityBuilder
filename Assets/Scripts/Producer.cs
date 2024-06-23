using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : MonoBehaviour
{
    [SerializeField] private Resources resourceType;
    [SerializeField] private int timeToProduce = 2;
    [SerializeField] private int multiplier = 1;
    [SerializeField] private int baseProduction = 10;

    private enum Resources
    {
        workers,
        money,
        power,
        water,
        clay,
        coal,
        bricks
    }

    private void Start()
    {
        StartCoroutine(Produce(resourceType, timeToProduce));
    }

    private IEnumerator Produce(Resources resourceType, int timeToProduce)
    {
        yield return new WaitForSeconds(timeToProduce);

        switch (resourceType)
        {
            case Resources.workers:
                ProduceWorkers();
                break;
            case Resources.money:
                ProduceMoney();
                break;
            case Resources.power:
                ProducePower();
                break;
            case Resources.water:
                ProduceWater();
                break;
            case Resources.clay:
                ProduceClay();
                break;
            case Resources.coal:
                ProduceCoal();
                break;
            case Resources.bricks:
                ProduceBricks();
                break;
            default:
                break;
        }

        UIManager.instance.UpdateStatsUI();

        StartCoroutine(Produce(resourceType, timeToProduce));
    }

    private bool CanProduce(List<int> inputs)
    {
        foreach (var resource in inputs)
        {
            if (resource <= 0)
            {
                return false;
            }
        }

        return true;
    }

    private void ProduceWorkers()
    {
        SimulationManager.instance.workers += baseProduction * multiplier;
    }

    private void ProduceMoney()
    {
        List<int> inputs = new List<int>
        {
            SimulationManager.instance.power,
            SimulationManager.instance.water
        };

        if (CanProduce(inputs))
        {
            SimulationManager.instance.power -= 10;
            SimulationManager.instance.water -= 10;

            SimulationManager.instance.money += baseProduction * multiplier;
        }
    }

    private void ProducePower()
    {
        List<int> inputs = new List<int>
        {
            SimulationManager.instance.workers,
            SimulationManager.instance.coal,
            SimulationManager.instance.water
        };

        if (CanProduce(inputs))
        {
            SimulationManager.instance.workers -= 10;
            SimulationManager.instance.coal -= 10;
            SimulationManager.instance.water -= 10;

            SimulationManager.instance.power += baseProduction * multiplier;
        }
    }

    private void ProduceWater()
    {
        SimulationManager.instance.water += baseProduction * multiplier;
    }

    private void ProduceClay()
    {
        List<int> inputs = new List<int>
        {
            SimulationManager.instance.workers
        };

        if (CanProduce(inputs))
        {
            SimulationManager.instance.workers -= 10;

            SimulationManager.instance.clay += baseProduction * multiplier;
        }
    }

    private void ProduceCoal()
    {
        List<int> inputs = new List<int>
        {
            SimulationManager.instance.workers
        };

        if (CanProduce(inputs))
        {
            SimulationManager.instance.workers -= 10;

            SimulationManager.instance.coal += baseProduction * multiplier;
        }
    }

    private void ProduceBricks()
    {
        List<int> inputs = new List<int>
        {
            SimulationManager.instance.power,
            SimulationManager.instance.water,
            SimulationManager.instance.clay
        };

        if (CanProduce(inputs))
        {
            SimulationManager.instance.power -= 10;
            SimulationManager.instance.water -= 10;
            SimulationManager.instance.clay -= 10;

            SimulationManager.instance.bricks += baseProduction * multiplier;
        }
    }
}