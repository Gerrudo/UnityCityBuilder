using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : Singleton<City>
{
    public int Day { get; private set; }
    [field: SerializeField] public int SecondsPerDay { get; private set; }
    public int Population { get; private set; }
    public int PopulationPool { get; private set; }
    public int Workers { get; private set; }
    public int WorkersPool { get; private set; }
    [field: SerializeField] public int Funds { get; private set; }
    public int Earnings { get; private set; }
    public int Expenses { get; private set; }
    public int Power { get; private set; }
    public int Water { get; private set; }
    public int Goods { get; private set; }
    public int ApprovalRating { get; private set; }

    private Dictionary<Vector3Int, IBuildable> cityTiles;

    private TileEditor tileEditor;
    private CityStatistics cityStatistics;

    protected override void Awake()
    {
        base.Awake();

        tileEditor = TileEditor.GetInstance();
        cityStatistics = CityStatistics.GetInstance();

        cityTiles = new Dictionary<Vector3Int, IBuildable>();
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
        
        foreach (var building in cityTiles.Values.ToList())
        {
            Debug.Log($"{building.Data.TileType}");
            
            building.Data.Taxes += Earnings;
        }
        
        StartCoroutine(UpdateCity());
    }

    public bool NewTile(Vector3Int tilePosition, GameTile gameTile)
    {
        if (Funds < gameTile.CostToBuild)
        {
            return false;
        }

        Funds -= gameTile.CostToBuild;

        PopulationPool += gameTile.MaxPopulation;
        
        IBuildingFactory buildingFactory = new BuildingFactory();

        var buildable = buildingFactory.CreateBuilding(gameTile.TileType);

        buildable.NewBuildingData(gameTile);

        cityTiles.Add(tilePosition, buildable);

        cityStatistics.UpdateUI();

        return true;
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        cityTiles.Remove(tilePosition);

        cityStatistics.UpdateUI();
    }
}    