using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        yield return new WaitForSeconds(10);

        day++;
        foreach (var tile in cityTiles)
        {
            CheckNetworkConnections(tile.Key);
            CalculateIncome(tile.Key);
            CheckForUpgrades(tile.Key);
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
        switch (cityTiles[tilePosition].TileType)
        {
            case TileType.Residential:
                tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Level1Tilebase);
                break;
            case TileType.Commerical:
                tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Level1Tilebase);
                break;
            case TileType.Industrial:
                tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Level1Tilebase);
                break;
            default:
                break;
        }
    }

    private void CheckNetworkConnections(Vector3Int tilePosition)
    {
        bool connectedToNetwork = false;
        Vector3Int[] neighbours = TilemapExtension.Neighbours(tilePosition);

        int i = 0;
        while(!connectedToNetwork || i == 3)
        {
            PlaceableTile connectedTile;

            if (cityTiles.TryGetValue(neighbours[i], out connectedTile))
            {
                if (connectedTile.TileType == TileType.Road)
                {
                    cityTiles[tilePosition].IsConnectedToRoad = true;

                    connectedToNetwork = true;

                    Debug.Log("Tile connected to road.");
                }
                else
                {
                    Debug.Log("Tile not connected to road.");
                }
            }
            else
            {
                Debug.Log($"No tile at {neighbours[i]}");
            }

            i++;
        }
    }
}