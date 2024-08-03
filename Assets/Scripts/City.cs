using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : Singleton<City>
{
    private Dictionary<Vector3Int, Building> cityTiles;
    private Dictionary<Guid, Citizen> citizens;
    
    private TileEditor tileEditor;
    private CityStatistics cityStatistics;
    
    public int Day { get; private set; }
    public int Population { get; private set; }
    public int Unemployed { get; private set; }
    public int Funds { get; private set; } = 4000000;
    public int Earnings { get; private set; }
    public int Power { get; private set; }
    public int Water { get; private set; }
    public int Goods { get; private set; }
    public int ApprovalRating { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        tileEditor = TileEditor.GetInstance();
        cityStatistics = CityStatistics.GetInstance();

        cityTiles = new Dictionary<Vector3Int, Building>();
        citizens = new Dictionary<Guid ,Citizen>();
    }

    private void Start()
    {
        StartCoroutine(UpdateCity());
        StartCoroutine(CountDays());
    }

    private void Update()
    {
        foreach (var tile in cityTiles)
        {
            tileEditor.DrawItem(tile.Key, tile.Value.TileBase);
        }
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(24);
            
        Day++;

        Funds += Earnings;
        Earnings = 0;

        StartCoroutine(CountDays());
    }
    
    private IEnumerator UpdateCity()
    {
        yield return new WaitForSeconds(1);
        
        DistributeCitizens();
        
        DistributeEmployees();
        
        foreach (var building in cityTiles)
        {
            building.Value.IsConnectedToRoad = CheckTileConnection(building.Key, TileType.Road);
            
            //No switch as it's possible for buildings to implement multiple of these interfaces.
            if (building.Value is IResidence residence)
            {
                //Updates the list of citizens that live at this tile
                residence.Residents = citizens.Values.Where(citizen => citizen.HomeTile == building.Key).ToList();
                
                Funds += residence.Residents.Count * 2;
            }
            
            if (building.Value is IEmployer employer)
            {
                //Updates the list of citizens that work at this tile
                employer.Jobs = citizens.Keys.Where(citizen => citizens[citizen].WorkTile == building.Key).ToList();
                
                Funds += employer.Jobs.Count * 2;
            }
        }
        
        Population = citizens.Count;
        Unemployed = citizens.Count(citizen => !citizen.Value.IsEmployed);
        
        cityStatistics.UpdateUI();
        
        StartCoroutine(UpdateCity());
    }

    private void DistributeCitizens()
    {
        foreach (var tile in cityTiles)
        {
            if (tile.Value is not IResidence residential) continue;
            if (residential.Residents.Count == residential.MaxPopulation) continue;
            
            var id = Guid.NewGuid();
                
            var newCitizen = new Citizen(tile.Key);
            
            citizens.Add(id, newCitizen);
        }
    }
    
    private void DistributeEmployees()
    {
        foreach (var tile in cityTiles)
        {
            if (tile.Value is not IEmployer employer) continue;
            if (employer.Jobs.Count == employer.MaxEmployees) continue;
            
            var unemployedCitizen = citizens.FirstOrDefault(citizen => !citizen.Value.IsEmployed);
            
            if (unemployedCitizen.Value == null) continue;
            
            citizens[unemployedCitizen.Key].WorkTile = tile.Key;
            citizens[unemployedCitizen.Key].IsEmployed = true;
            
            employer.Jobs.Add(unemployedCitizen.Key);
        }
    }

    public bool CanPlaceNewTile(Preset buildingPreset)
    {
        return Funds >= buildingPreset.CostToBuild;
    }

    public void NewTile(Vector3Int tilePosition, Preset buildingPreset)
    {
        Funds -= buildingPreset.CostToBuild;
        
        var buildingFactory = new BuildingFactory();

        var buildable = buildingFactory.CreateBuilding(buildingPreset);

        cityTiles.Add(tilePosition, buildable);

        cityStatistics.UpdateUI();
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        cityTiles.Remove(tilePosition);

        cityStatistics.UpdateUI();
    }

    public Building GetBuildingData(Vector3Int tilePosition)
    {
        cityTiles.TryGetValue(tilePosition, out var building);
        
        return building;
    }
    
    private bool CheckTileConnection(Vector3Int tilePosition, TileType tileToCheck)
    {
        var connected = false;
        
        var neighbours = TilemapExtension.Neighbours(tilePosition);

        foreach (var neighbour in neighbours)
        {
            if (!cityTiles.TryGetValue(neighbour, out var connectedTile)) continue;
            
            if (connectedTile.TileType != tileToCheck) continue;
            connected = true;
            
            break;
        }

        return connected;
    }
}    