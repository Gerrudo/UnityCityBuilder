using System;
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
        
        ResetValues();
        
        foreach (var building in cityTiles)
        {
            building.Value.Data.IsConnectedToRoad = CheckTileConnection(building.Key, TileType.Road);
            
            building.Value.UpdateBuilding();
            
            UpgradeBuilding(building.Key);

            GenerateCitizen(building.Key, building.Value);
            
            SumValues(building.Value);
        }
        
        cityStatistics.UpdateUI();
        
        StartCoroutine(UpdateCity());
    }
    
    private static void ResetValues()
    {
        CityData.Population = 0;
        CityData.Earnings = 0;
        CityData.Unemployed = 0;
        CityData.Power = 0;
        CityData.Water = 0;
        CityData.Goods = 0;
    }

    private void SumValues(IBuildable building)
    {
        //Better way to do this? Maybe we can just add all the values in our building class then do a sum?
        CityData.Population += building.Data.CurrentPopulation;

        CityData.Unemployed += building.Data.Unemployed;
        CityData.Unemployed -= building.Data.Employees;

        CityData.Power -= building.Data.PowerInput;
        CityData.Power += building.Data.PowerOutput;
            
        CityData.Water -= building.Data.WaterInput;
        CityData.Water += building.Data.WaterOutput;
            
        CityData.Goods -= building.Data.GoodsInput;
        CityData.Goods += building.Data.GoodsOutput;
            
        CityData.Earnings -= building.Data.Expenses;
        CityData.Earnings += building.Data.Taxes;
    }

    private void GenerateCitizen(Vector3Int tilePosition, IBuildable building)
    {
        if (building.Data.TileType != TileType.Residential) return;
        if (building.Data.CurrentPopulation > building.Data.MaxPopulation) return;
        
        var newCitizen = new Citizen(tilePosition);
        building.Data.Residents.Add(newCitizen);
    }

    public bool NewTile(Vector3Int tilePosition, Preset buildingPreset)
    {
        if (CityData.Funds < buildingPreset.CostToBuild)
        {
            return false;
        }

        CityData.Funds -= buildingPreset.CostToBuild;
        
        IBuildingFactory buildingFactory = new BuildingFactory();

        var buildable = buildingFactory.CreateBuilding(buildingPreset.TileType);

        buildable.NewBuildingData(buildingPreset);

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