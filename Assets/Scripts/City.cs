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
        yield return new WaitForSeconds(1);

        //Values to be recalculated
        Population = 0;
        Power = 0;
        Water = 0;

        foreach (var tile in cityTiles)
        {
            ProcessTile(tile.Key);
        }

        cityStatistics.UpdateUI();

        StartCoroutine(UpdateCity());
    }

    public bool NewTile(Vector3Int tilePosition, PlaceableTile tile)
    {
        if (Funds < tile.CostToBuild)
        {
            Debug.Log("Cannot afford to place tile.");

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

    private void CalculatePopulation(Vector3Int tilePosition)
    {
        bool isMaxPopulation = cityTiles[tilePosition].CurrentPopulation >= cityTiles[tilePosition].MaxPopulation;

        if (!isMaxPopulation)
        {
            cityTiles[tilePosition].CurrentPopulation += 10;
        }

        Population += cityTiles[tilePosition].CurrentPopulation;
    }

    private void CalculateWater(Vector3Int tilePosition)
    {
        Water += 100;
    }

    private void CalculatePower(Vector3Int tilePosition)
    {
        Power += 100;
    }

    private void CalculateUtilityUsage(Vector3Int tilePosition)
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

    private void CalculateIncome(Vector3Int tilePosition)
    {
        Earnings += 100;
    }

    private void CalculateExpenses(Vector3Int tilePosition)
    {
        Earnings -= 10;
    }

    private void SellGoods(Vector3Int tilePosition)
    {
        Goods -= 50;
        Earnings += 100;
    }

    private void ProduceGoods(Vector3Int tilePosition)
    {
        Goods += 100;
    }

    private void UpgradeBuilding(Vector3Int tilePosition)
    {
        tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Level1Tilebase);
    }

    private void ProcessTile(Vector3Int tilePosition)
    {
        CheckRoadConnection(tilePosition);

        if (cityTiles[tilePosition].IsConnectedToRoad)
        {
            bool hasUtilites = cityTiles[tilePosition].IsPowered && cityTiles[tilePosition].IsWatered;

            switch (cityTiles[tilePosition].TileType)
            {
                case TileType.Residential:
                    UpgradeBuilding(tilePosition);
                    CalculateUtilityUsage(tilePosition);
                    CalculatePopulation(tilePosition);

                    if (hasUtilites)
                    {
                        CalculateIncome(tilePosition);
                    }

                    CalculateExpenses(tilePosition);

                    break;
                case TileType.Commerical:
                    UpgradeBuilding(tilePosition);
                    CalculateUtilityUsage(tilePosition);

                    if (hasUtilites)
                    {
                        SellGoods(tilePosition);
                        CalculateIncome(tilePosition);
                    }

                    CalculateExpenses(tilePosition);

                    break;
                case TileType.Industrial:
                    UpgradeBuilding(tilePosition);
                    CalculateUtilityUsage(tilePosition);

                    if (hasUtilites)
                    {
                        ProduceGoods(tilePosition);
                        CalculateIncome(tilePosition);
                    }

                    CalculateExpenses(tilePosition);

                    break;
                case TileType.Generator:
                    CalculatePower(tilePosition);
                    CalculateExpenses(tilePosition);
                    break;
                case TileType.WaterTower:
                    CalculateWater(tilePosition);
                    CalculateExpenses(tilePosition);

                    break;
                default:
                    CalculateExpenses(tilePosition);

                    break;
            }
        }
        else
        {
            Debug.Log($"{cityTiles[tilePosition].TileType} is not connected to a road!");
        }
    }
}