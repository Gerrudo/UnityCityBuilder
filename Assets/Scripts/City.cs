using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : Singleton<City>
{
    private int day;
    [SerializeField] private int secondsPerDay;
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

    private Dictionary<Vector3Int, PlaceableTile> cityTiles;

    TileEditor tileEditor;
    CityStatistics cityStatistics;

    protected override void Awake()
    {
        base.Awake();

        tileEditor = TileEditor.GetInstance();
        cityStatistics = CityStatistics.GetInstance();

        cityTiles = new Dictionary<Vector3Int, PlaceableTile>();
    }

    private void Start()
    {
        StartCoroutine(CountDays());
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(secondsPerDay);

        day++;

        //Values to be recalculated
        population = 0;

        foreach (var tile in cityTiles)
        {
            RequiredChecks(tile.Key);
        }

        cityStatistics.UpdateUI();

        StartCoroutine(CountDays());
    }

    public bool NewTile(Vector3Int tilePosition, PlaceableTile tile)
    {
        if (money < tile.CostToBuild)
        {
            Debug.Log("Cannot afford to place tile.");

            return false;
        }
        
        money -= tile.CostToBuild;

        cityTiles.Add(tilePosition, tile);

        cityStatistics.UpdateUI();

        return true;
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        cityTiles.Remove(tilePosition);
    }

    private void CalculateIncome(Vector3Int tilePosition)
    {
        if (cityTiles[tilePosition].TileType == TileType.Commerical)
        {
            money += cityTiles[tilePosition].MoneyPerDay;
        }
    }

    private void CheckForUpgrades(Vector3Int tilePosition)
    {
        tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Level1Tilebase);
    }

    private void CheckNetworkConnections(Vector3Int tilePosition)
    {
        Vector3Int[] neighbours = TilemapExtension.Neighbours(tilePosition);

        for (int i = 0; i < neighbours.Length; i++)
        {
            PlaceableTile connectedTile;

            cityTiles[tilePosition].IsConnectedToRoad = false;

            if (cityTiles.TryGetValue(neighbours[i], out connectedTile))
            {
                if (connectedTile.TileType == TileType.Road)
                {
                    cityTiles[tilePosition].IsConnectedToRoad = true;

                    break;
                }
            }
        }
    }

    private void CalculatePopulation(Vector3Int tilePosition)
    {
        bool isMaxPopulation = cityTiles[tilePosition].CurrentPopulation > cityTiles[tilePosition].MaxPopulation;

        if (!isMaxPopulation)
        {
            cityTiles[tilePosition].CurrentPopulation += 10;
        }

        population += cityTiles[tilePosition].CurrentPopulation;
    }

    private void RequiredChecks(Vector3Int tilePosition)
    {
        CheckNetworkConnections(tilePosition);

        switch (cityTiles[tilePosition].TileType)
        {
            case TileType.Residential:
                if (cityTiles[tilePosition].IsConnectedToRoad)
                {
                    CheckForUpgrades(tilePosition);
                    CalculatePopulation(tilePosition);
                    CalculateIncome(tilePosition);
                }

                break;
            case TileType.Commerical:
                if (cityTiles[tilePosition].IsConnectedToRoad)
                {
                    CheckForUpgrades(tilePosition);
                    CalculateIncome(tilePosition);
                }

                break;
            case TileType.Industrial:
                if (cityTiles[tilePosition].IsConnectedToRoad)
                {
                    CheckForUpgrades(tilePosition);
                    CalculateIncome(tilePosition);
                }

                break;
            default:
                break;
        }
    }
}