using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class City : Singleton<City>
{
    public Dictionary<Vector3Int, Building> CityTiles { get; private set; }
    private Dictionary<Guid, Citizen> citizens;
    
    private TileEditor tileEditor;
    private CityStatistics cityStatistics;

    public CityData CityData;

    protected override void Awake()
    {
        base.Awake();

        tileEditor = TileEditor.GetInstance();
        cityStatistics = CityStatistics.GetInstance();

        CityData = new CityData();
        CityTiles = new Dictionary<Vector3Int, Building>();
        citizens = new Dictionary<Guid ,Citizen>();
    }

    private void Start()
    {
        StartCoroutine(UpdateCity());
        StartCoroutine(CountDays());
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(CityData.SecondsPerDay);
        
        DistributeCitizens();
            
        CityData.Day++;

        CityData.Funds += CityData.Earnings;

        StartCoroutine(CountDays());
    }
    
    private IEnumerator UpdateCity()
    {
        yield return new WaitForSeconds(CityData.SecondsPerUpdate);

        ResetValues();
        
        DistributeEmployees();
        
        foreach (var building in CityTiles.ToArray())
        {
            ProcessBuilding(building.Key, building.Value);
        }
        
        foreach (var building in CityTiles.ToArray())
        {
            //Status must be checked after all other checks to have an updated values for Power/Water.
            building.Value.UpdateBuildingStatus(CityData);
        }

        if (CityData.ApprovalRating != 0)
        {
            CityData.ApprovalRating = Calculations.Normalise(CityData.ApprovalRating, CityTiles.Count(tile => tile.Value is IApprovable));   
        }
        
        CityData.Population = citizens.Count;
        CityData.Unemployed = citizens.Count(citizen => !citizen.Value.IsEmployed);
        
        cityStatistics.UpdateUI();
        
        StartCoroutine(UpdateCity());
    }

    private void ProcessBuilding(Vector3Int tilePosition, Building building)
    {
        //TODO: Can be moved into UpdateBuildingStatus()
        building.IsConnectedToRoad = TilemapExtension.CheckTileConnection(tilePosition, TileType.Road, CityTiles);
        
        if (building is Residential residential)
        {
            //Updates the list of citizens that live at this tile
            residential.Residents = citizens.Values.Where(citizen => citizen.HomeTile == tilePosition).ToList();
        }
            
        if (building is IEmployable employable)
        {
            //Updates the list of citizens that work at this tile
            employable.Jobs = citizens.Keys.Where(citizen => citizens[citizen].WorkTile == tilePosition).ToList();
        }
        
        building.UpdateBuilding(CityData);

        if (building is IGrowable growable && growable.CanUpgrade())
        {
            //TODO: Can possibly be moved into the growable method.
            //TODO: Find a better way to do this overall.
            tileEditor.defaultMap.SetTile(tilePosition, building.TileBase);
        }
        
        if (building is IApprovable approvable)
        {
            CityData.ApprovalRating += approvable.GetApprovalScore(CityTiles);
        }
    }

    private void ResetValues()
    {
        CityData.Power = 0;
        CityData.Water = 0;
        CityData.Earnings = 0;
        CityData.Goods = 0;
    }
    
    private void DistributeCitizens()
    {
        foreach (var tile in CityTiles)
        {
            if (tile.Value is not Residential residential) continue;
            if (residential.Residents.Count == residential.MaxPopulation) continue;
            if (!tile.Value.IsActive) continue;

            var random = new Random();
            var residentsToAdd = random.Next(1, 5);
            residentsToAdd *= residential.GetPopulationMultiplier(CityTiles);

            for (var i = 0; i < residentsToAdd; i++)
            {
                if (residential.Residents.Count == residential.MaxPopulation) break;
                
                var id = Guid.NewGuid();
                
                var newCitizen = new Citizen(tile.Key);
            
                citizens.Add(id, newCitizen);
            }
        }
    }
    
    private void DistributeEmployees()
    {
        foreach (var tile in CityTiles)
        {
            if (tile.Value is not IEmployable employer) continue;
            if (employer.Jobs.Count == employer.MaxEmployees) continue;
            if (!tile.Value.IsActive) continue;
            
            var unemployedCitizen = citizens.FirstOrDefault(citizen => !citizen.Value.IsEmployed);
            
            if (unemployedCitizen.Value == null) continue;
            
            citizens[unemployedCitizen.Key].WorkTile = tile.Key;
            citizens[unemployedCitizen.Key].IsEmployed = true;
            
            employer.Jobs.Add(unemployedCitizen.Key);
        }
    }

    public bool CanPlaceNewTile(BuildingPreset buildingPreset)
    {
        return CityData.Funds >= buildingPreset.CostToBuild;
    }

    public void NewTile(Vector3Int tilePosition, BuildingPreset buildingPreset)
    {
        CityData.Funds -= buildingPreset.CostToBuild;
        
        //TODO: Can this be moved into Awake() so we don't create a new building Factory class everytime?
        IBuildingFactory buildingFactory = new BuildingFactory();

        var buildable = buildingFactory.CreateBuilding(buildingPreset);

        //Need to replace if the tile already exists
        if (CityTiles.TryGetValue(tilePosition, out _))
        {
            CityTiles[tilePosition] = buildable;
        }
        else
        {
            CityTiles.Add(tilePosition, buildable);   
        }

        cityStatistics.UpdateUI();
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        CityTiles.Remove(tilePosition);

        cityStatistics.UpdateUI();
    }
}    