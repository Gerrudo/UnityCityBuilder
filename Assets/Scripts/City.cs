using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class City : Singleton<City>
{
    private Dictionary<Vector3Int, IBuildable> cityTiles;
    private List<Citizen> citizens;
    
    private TileEditor tileEditor;
    private CityStatistics cityStatistics;

    protected override void Awake()
    {
        base.Awake();

        tileEditor = TileEditor.GetInstance();
        cityStatistics = CityStatistics.GetInstance();

        cityTiles = new Dictionary<Vector3Int, IBuildable>();
        citizens = new List<Citizen>();
    }

    private void Start()
    {
        StartCoroutine(UpdateCity());
        StartCoroutine(CountDays());
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(24);

        DistributeCitizens();
            
        CityData.Day++;

        CityData.Funds += CityData.Earnings;
        CityData.Earnings = 0;

        StartCoroutine(CountDays());
    }
    
    private IEnumerator UpdateCity()
    {
        yield return new WaitForSeconds(1);
        
        ResetValues();

        CityData.Population = citizens.Count;
        CityData.Unemployed = citizens.Count(citizen => !citizen.IsEmployed);
        
        foreach (var building in cityTiles)
        {
            building.Value.Data.IsConnectedToRoad = CheckTileConnection(building.Key, TileType.Road);
            
            //TODO: Can pass CityData to our UpdateBuilding() method so it can do this check within the class.
            if (building.Value.Data.TileType == TileType.Residential)
            {
                building.Value.Data.Residents = citizens.Where(citizen => citizen.HomeTile == building.Key).ToList();   
            }
            
            building.Value.UpdateBuilding();
            
            UpgradeBuilding(building.Key);
            
            SumValues(building.Value);
        }
        
        cityStatistics.UpdateUI();
        
        StartCoroutine(UpdateCity());
    }
    
    private static void ResetValues()
    {
        CityData.Earnings = 0;
        CityData.Power = 0;
        CityData.Water = 0;
        CityData.Goods = 0;
    }

    private void SumValues(IBuildable building)
    {
        //TODO: Look at using .Sum from linq instead of resetting and re-adding.
        //Somewhat like this: CityData.Power = cityTiles.Values.Sum(building => building.Data.PowerOutput);
        CityData.Power -= building.Data.PowerInput;
        CityData.Power += building.Data.PowerOutput;
            
        CityData.Water -= building.Data.WaterInput;
        CityData.Water += building.Data.WaterOutput;
            
        CityData.Goods -= building.Data.GoodsInput;
        CityData.Goods += building.Data.GoodsOutput;
            
        CityData.Earnings -= building.Data.Expenses;
        CityData.Earnings += building.Data.Taxes;
    }

    private void DistributeCitizens()
    {
        foreach (var tile in cityTiles)
        {
            if (tile.Value.Data.TileType != TileType.Residential) continue;
            if (tile.Value.Data.CurrentPopulation == tile.Value.Data.MaxPopulation) continue;
                
            var newCitizen = new Citizen(tile.Key);
            
            citizens.Add(newCitizen);
        }
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
    
    //TODO: Move to interface
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
    
    //TODO: Move to interface
    private void UpgradeBuilding(Vector3Int tilePosition)
    {
        if (cityTiles[tilePosition].Data.BuildingLevel != 1) return;
        
        if (cityTiles[tilePosition].Data.TileType is TileType.Residential or TileType.Commercial or TileType.Industrial)
        {
            tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Data.Level1TilBase);
        }
    }
}    