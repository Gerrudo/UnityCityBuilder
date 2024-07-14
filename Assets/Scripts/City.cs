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
    public int PopulationPool { get; private set; }
    [field: SerializeField]
    public int Funds { get; private set; }
    public int Earnings { get; private set; }
    public int Expenses { get; private set; }
    public int Power { get; private set; }
    public int Water { get; private set; }
    public int Goods { get; private set; }
    public int ApprovalRating { get; private set; }

    private Dictionary<Vector3Int, PlaceableTile> cityTiles;

    private TileEditor tileEditor;
    private CityStatistics cityStatistics;

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
            cityTiles[key].IsConnectedToRoad = CheckRoadConnection(key);

            if (!cityTiles[key].IsConnectedToRoad) continue;

            UpgradeBuilding(key);

            cityTiles[key] = TileCalculations(cityTiles[key]);
        }

        UpdateCity();
        
        CalculateApprovalRating();
        
        cityStatistics.UpdateUI();
        
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
    }

    public bool NewTile(Vector3Int tilePosition, PlaceableTile tile)
    {
        if (Funds < tile.CostToBuild)
        {
            return false;
        }

        Funds -= tile.CostToBuild;

        PopulationPool += tile.MaxPopulation;

        cityTiles.Add(tilePosition, tile);

        cityStatistics.UpdateUI();

        return true;
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        PopulationPool -= cityTiles[tilePosition].MaxPopulation;
        
        cityTiles.Remove(tilePosition);

        cityStatistics.UpdateUI();
    }

    private bool CheckRoadConnection(Vector3Int tilePosition)
    {
        var connected = false;
        var neighbours = TilemapExtension.Neighbours(tilePosition);

        foreach (var neighbour in neighbours)
        {
            if (!cityTiles.TryGetValue(neighbour, out var connectedTile)) continue;

            if (connectedTile.TileType != TileType.Road) continue;
                
            connected = true;
            
            break;
        }

        return connected;
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
        var approval = 0;
        var votes = cityTiles.Count;

        if (votes == 0) return;
        
        foreach (var tile in cityTiles)
        {
            if (tile.Value.Happiness == 3)
            {
                approval++;
            }
        }

        ApprovalRating = (int)Math.Round(((double)approval / votes) * 100);
    }

    private static PlaceableTile TileCalculations(PlaceableTile tile)
    {
        switch (tile.TileType)
        {
            case TileType.Road:
                return tile;
            case TileType.Residential:
                tile.CurrentPopulation = Calculations.GetPopulation(tile.MaxPopulation, tile.CurrentPopulation);
                tile.WaterDemand = Calculations.ConsumeWater(tile.CurrentPopulation);
                tile.PowerDemand = Calculations.ConsumePower(tile.CurrentPopulation);
                tile.Taxes = Calculations.PayTaxes(tile.CurrentPopulation);
                return tile;
            case TileType.Commercial:
                tile.WaterDemand = Calculations.ConsumeWater(tile.CurrentPopulation);
                tile.PowerDemand = Calculations.ConsumePower(tile.CurrentPopulation);
                tile.Taxes = Calculations.PayTaxes(tile.CurrentPopulation);
                return tile;
            case TileType.Industrial:
                tile.WaterDemand = Calculations.ConsumeWater(tile.CurrentPopulation);
                tile.PowerDemand = Calculations.ConsumePower(tile.CurrentPopulation);
                tile.Taxes = Calculations.PayTaxes(tile.CurrentPopulation);
                return tile;
            case TileType.Generator:
                tile.PowerProduction = Calculations.GeneratePower(tile.Workers, tile.MaxWorkers);
                return tile;
            case TileType.WaterTower:
                return tile;
            default:
                return tile;
        }
    }
}