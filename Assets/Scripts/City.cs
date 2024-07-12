using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Singleton<City>
{
    public int Day { get; private set; }
    [field: SerializeField]
    public int SecondsPerDay { get; private set; }
    public int Population { get; private set; }
    [field: SerializeField]
    public int Funds { get; private set; }
    public int Earnings { get; private set; }
    public int Power { get; private set; }
    public int Water { get; private set; }
    public int Goods { get; private set; }
    public int ApprovalRating { get; private set; }

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
        StartCoroutine(UpdateCity());
        StartCoroutine(CountDays());
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(SecondsPerDay);

        Day++;

        Funds += Earnings;
        Earnings = 0;

        cityStatistics.UpdateUI();

        StartCoroutine(CountDays());
    }

    private IEnumerator UpdateCity()
    {
        yield return new WaitForSeconds(2);

        //Values to be recalculated
        Population = 0;

        foreach (var tile in cityTiles)
        {
            TileCalculations(tile.Key);
        }

        CalculateApprovalRating();

        cityStatistics.UpdateUI();

        StartCoroutine(UpdateCity());
    }

    public bool NewTile(Vector3Int tilePosition, PlaceableTile tile)
    {
        if (Funds < tile.CostToBuild)
        {
            return false;
        }

        Funds -= tile.CostToBuild;

        cityTiles.Add(tilePosition, tile);

        cityStatistics.UpdateUI();

        return true;
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        cityTiles.Remove(tilePosition);

        cityStatistics.UpdateUI();
    }

    private void CheckRoadConnection(Vector3Int tilePosition)
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

    private void CheckUtilities(Vector3Int tilePosition)
    {
        Power -= cityTiles[tilePosition].PowerDemand;
        Water -= cityTiles[tilePosition].WaterDemand;

        if (Power > cityTiles[tilePosition].PowerDemand)
        {
            cityTiles[tilePosition].IsPowered = true;
        }
        else
        {
            cityTiles[tilePosition].IsPowered = false;
        }

        if (Water > cityTiles[tilePosition].WaterDemand)
        {
            cityTiles[tilePosition].IsWatered = true;
        }
        else
        {
            cityTiles[tilePosition].IsWatered = false;
        }
    }

    private void UpgradeBuilding(Vector3Int tilePosition)
    {
        tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Level1Tilebase);
    }

    private void CalculateApprovalRating()
    {
        int approval = 0;
        int votes = cityTiles.Count;

        if (votes != 0)
        {
            foreach (var tile in cityTiles)
            {
                if (tile.Value.Happiness == 3)
                {
                    approval++;
                }
            }

            ApprovalRating = (int)Math.Round(((double)approval / (double)votes) * 100);
        }
    }

    private void TileCalculations(Vector3Int tilePosition)
    {
        ICalculable calculable = new Placeable();

        PlaceableTile updatedTile = calculable.Calculate(cityTiles[tilePosition]);

        cityTiles[tilePosition] = updatedTile;
    }
}