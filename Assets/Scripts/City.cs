using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int Expenses { get; private set; }
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
        StartCoroutine(UpdateTiles());
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

    private IEnumerator UpdateTiles()
    {
        yield return new WaitForSeconds(1);
        
        foreach (var key in cityTiles.Keys.ToList())
        {
            CheckRoadConnection(key);

            if (!cityTiles[key].IsConnectedToRoad) continue;

            UpgradeBuilding(key);
                
            TileCalculations(key);
        }

        UpdateCity();
        
        StartCoroutine(UpdateTiles());
    }

    private void UpdateCity()
    {
        Population = 0;
        Water = 0;
        Power = 0;
        Earnings = 0;
        Expenses = 0;
        
        foreach (var tile in cityTiles.Values.ToList())
        {
            Expenses += tile.Expenses;
            
            if (!tile.IsConnectedToRoad) continue;
            
            Water -= tile.WaterDemand;
            Water += tile.WaterProduction;
            Power -= tile.PowerDemand;
            Power += tile.PowerProduction;
            Population += tile.CurrentPopulation;
            Earnings += tile.Taxes;
        }

        Earnings -= Expenses;
        
        CalculateApprovalRating();
        
        cityStatistics.UpdateUI();
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
        var neighbours = TilemapExtension.Neighbours(tilePosition);

        foreach (var neighbour in neighbours)
        {
            cityTiles[tilePosition].IsConnectedToRoad = false;

            if (cityTiles.TryGetValue(neighbour, out var connectedTile))
            {
                if (connectedTile.TileType == TileType.Road)
                {
                    cityTiles[tilePosition].IsConnectedToRoad = true;

                    break;
                }
            }
        }
    }

    private void UpgradeBuilding(Vector3Int tilePosition)
    {
        if (cityTiles[tilePosition].TileType is TileType.Residential or TileType.Commercial or TileType.Industrial)
        {
            tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Level1Tilebase);
        }
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

        cityTiles[tilePosition] = calculable.Calculate(cityTiles[tilePosition]);
    }
}