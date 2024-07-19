using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Singleton<City>
{
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
        yield return new WaitForSeconds(24);

        CityData.Day++;

        CityData.Funds += CityData.Earnings;
        CityData.Earnings = 0;

        StartCoroutine(CountDays());
    }
    
    private IEnumerator UpdateCity()
    {
        yield return new WaitForSeconds(1);
        
        //Better way to do this?
        CityData.Population = 0;
        CityData.Earnings = 0;
        CityData.Unemployed = 0;
        CityData.Power = 0;
        CityData.Water = 0;
        CityData.Goods = 0;
        
        foreach (var building in cityTiles)
        {
            building.Value.Data.IsConnectedToRoad = CheckTileConnection(building.Key, TileType.Road);
            
            building.Value.UpdateBuilding();
            
            UpgradeBuilding(building.Key);
            
            //Better way to do this?
            CityData.Population += building.Value.Data.CurrentPopulation;

            CityData.Unemployed += building.Value.Data.Unemployed;
            CityData.Unemployed -= building.Value.Data.Employees;

            CityData.Power -= building.Value.Data.PowerConsumption;
            CityData.Power += building.Value.Data.PowerProduction;
            
            CityData.Water -= building.Value.Data.WaterConsumption;
            CityData.Water += building.Value.Data.WaterProduction;
            
            CityData.Goods -= building.Value.Data.GoodsConsumption;
            CityData.Goods += building.Value.Data.GoodsProduction;
            
            CityData.Earnings -= building.Value.Data.Expenses;
            CityData.Earnings += building.Value.Data.Taxes;
        }
        
        cityStatistics.UpdateUI();
        
        StartCoroutine(UpdateCity());
    }

    public bool NewTile(Vector3Int tilePosition, GameTile gameTile)
    {
        if (CityData.Funds < gameTile.CostToBuild)
        {
            return false;
        }

        CityData.Funds -= gameTile.CostToBuild;
        
        IBuildingFactory buildingFactory = new BuildingFactory();

        var buildable = buildingFactory.CreateBuilding(gameTile.TileType);

        buildable.NewBuildingData(gameTile);

        cityTiles.Add(tilePosition, buildable);

        cityStatistics.UpdateUI();

        return true;
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        cityTiles[tilePosition].DestroyBuilding();
        
        cityTiles.Remove(tilePosition);

        cityStatistics.UpdateUI();
    }
    
    //Should be moved to interface
    private bool CheckTileConnection(Vector3Int tilePosition, TileType tileToCheck)
    {
        var connected = false;
        
        var neighbours = TilemapExtension.Neighbours(tilePosition);

        foreach (var neighbour in neighbours)
        {
            if (!cityTiles.TryGetValue(neighbour, out var connectedTile)) continue;
                if (connectedTile.Data.TileType != tileToCheck) continue;
                    connected = true;
                    break;
        }

        return connected;
    }
    
    //Should be moved to interface
    private void UpgradeBuilding(Vector3Int tilePosition)
    {
        if (cityTiles[tilePosition].Data.BuildingLevel != 1) return;
        
        if (cityTiles[tilePosition].Data.TileType is TileType.Residential or TileType.Commercial or TileType.Industrial)
        {
            tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Data.Level1TilBase);
        }
    }
}    