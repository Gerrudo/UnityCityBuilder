using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Singleton<City>
{
    public int Day { get; private set; }

    [field: SerializeField]
    public int SecondsPerDay { get; private set; }

    [field: SerializeField]
    public int Population { get; private set; }

    [field: SerializeField]
    public int Money { get; private set; }

    public int Power { get; private set; }

    public int Water { get; private set; }

    public int Coal { get; private set; }

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
        yield return new WaitForSeconds(SecondsPerDay);

        Day++;

        //Values to be recalculated
        Population = 0;

        foreach (var tile in cityTiles)
        {
            RequiredChecks(tile.Key);
        }

        cityStatistics.UpdateUI();

        StartCoroutine(CountDays());
    }

    public bool NewTile(Vector3Int tilePosition, PlaceableTile tile)
    {
        if (Money < tile.CostToBuild)
        {
            Debug.Log("Cannot afford to place tile.");

            return false;
        }
        
        Money -= tile.CostToBuild;

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
        if (cityTiles[tilePosition].TileType is TileType.Residential or TileType.Commerical or TileType.Industrial)
        {
            Money += cityTiles[tilePosition].Taxes;
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
        bool isMaxPopulation = cityTiles[tilePosition].CurrentPopulation >= cityTiles[tilePosition].MaxPopulation;

        if (!isMaxPopulation)
        {
            cityTiles[tilePosition].CurrentPopulation += 10;
        }

        Population += cityTiles[tilePosition].CurrentPopulation;
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