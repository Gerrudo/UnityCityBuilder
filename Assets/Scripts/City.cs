using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Singleton<City>
{
    private int day;
    [SerializeField] private int money;
    private int power;
    private int water;
    private int population;
    private int coal;

    #region Get
    public int Day
    {
        get
        {
            return day;
        }
    }

    public int Money
    {
        get
        {
            return money;
        }
    }

    public int Power
    {
        get
        {
            return power;
        }
    }

    public int Water
    {
        get
        {
            return water;
        }
    }

    public int Population
    {
        get
        {
            return population;
        }
    }

    public int Coal
    {
        get
        {
            return coal;
        }
    }
    #endregion

    private Dictionary<Vector3, PlaceableTile> cityTiles;

    CityStatistics cityStatistics;

    protected override void Awake()
    {
        base.Awake();

        cityStatistics = CityStatistics.GetInstance();

        cityTiles = new Dictionary<Vector3, PlaceableTile>();
    }

    private void Start()
    {
        StartCoroutine(CountDays());
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(10);

        day++;
        CalculateIncome();

        cityStatistics.UpdateUI();

        StartCoroutine(CountDays());
    }

    public bool NewTile(Vector3 tilePositon, PlaceableTile tile)
    {
        if (money < tile.CostToBuild)
        {
            Debug.Log("Cannot afford to place tile.");

            return false;
        }
        
        money -= tile.CostToBuild;

        cityTiles.Add(tilePositon, tile);

        cityStatistics.UpdateUI();

        return true;
    }

    private void CalculateIncome()
    {
        if (cityTiles.Count >= 1)
        {
            foreach (var tile in cityTiles)
            {
                if (tile.Value.TileType == TileType.Commerical)
                {
                    money += tile.Value.MoneyPerDay;
                }
            }

            cityStatistics.UpdateUI();
        }
    }
}