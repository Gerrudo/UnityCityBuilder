using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class City : Singleton<City>
{
    private Dictionary<Vector3Int, Building> cityTiles;
    private Dictionary<Guid, Citizen> citizens;
    
    private TileEditor tileEditor;
    private CityStatistics cityStatistics;
    
    public int Day { get; private set; }
    [field: SerializeField] public int SecondsPerDay { get; private set; }
    [field: SerializeField] public int SecondsPerUpdate { get; private set; }
    public int Population { get; private set; }
    public int Unemployed { get; private set; }
    [field: SerializeField] public int Funds { get; private set; }
    public int Earnings { get; private set; }
    public int Power { get; private set; }
    public int Water { get; private set; }
    public int Goods { get; private set; }
    public float ApprovalRating { get; private set; }

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
    
    private void DrawCity()
    {
        foreach (var tile in cityTiles)
        {
            tileEditor.DirectDraw(tileEditor.defaultMap, tile.Key, tile.Value.TileBase);
        }
    }

    private IEnumerator CountDays()
    {
        yield return new WaitForSeconds(SecondsPerDay);
        
        DistributeCitizens();
            
        Day++;

        Funds += Earnings;

        StartCoroutine(CountDays());
    }
    
    private IEnumerator UpdateCity()
    {
        yield return new WaitForSeconds(SecondsPerUpdate);

        ResetValues();
        
        DistributeEmployees();
        
        foreach (var building in cityTiles.ToList())   
        {
            GetBuildingValues(building.Key ,building.Value);
        }

        ApprovalRating = Calculations.Normalise(ApprovalRating, cityTiles.Count(tile => tile.Value is IApproval));
        
        Population = citizens.Count;
        Unemployed = citizens.Count(citizen => !citizen.Value.IsEmployed);
        
        cityStatistics.UpdateUI();

        DrawCity();
        
        StartCoroutine(UpdateCity());
    }

    private void GetBuildingValues(Vector3Int tilePosition, Building building)
    {
        //TODO: Can be moved into UpdateBuildingStatus()
        building.IsConnectedToRoad = TilemapExtension.CheckTileConnection(tilePosition, TileType.Road, cityTiles);
        
        building.UpdateBuildingStatus();
        
        //No switch as it's possible for buildings to implement multiple of these interfaces.
        if (building is IResidence residence)
        {
            //Updates the list of citizens that live at this tile
            residence.Residents = citizens.Values.Where(citizen => citizen.HomeTile == tilePosition).ToList();
        }
            
        if (building is IEmployer employer)
        {
            //Updates the list of citizens that work at this tile
            employer.Jobs = citizens.Keys.Where(citizen => citizens[citizen].WorkTile == tilePosition).ToList();
        }
        
        if (building is IWater water)
        {
            Water = water.GenerateWater(Water);
            Water = water.ConsumeWater(Water);
        }
        
        if (building is IPower power)
        {
            Power = power.GeneratePower(Power);
            Power = power.ConsumePower(Power);
        }

        if (building is IEarnings earnings)
        {
            Earnings += earnings.GenerateEarnings();
            Earnings -= earnings.ConsumeEarnings();
        }

        if (building is IGoods goods)
        {
            Goods += goods.GenerateGoods();
            Goods -= goods.ConsumeGoods();
        }

        if (building is IApproval approval)
        {
            ApprovalRating += approval.GetApprovalScore(cityTiles);
        }
        
        if (building is IGrowable growable)
        {
            growable.CanUpgrade();
            
            tileEditor.DirectDraw(tileEditor.defaultMap, tilePosition, growable.TileBase);
        }
    }

    private void ResetValues()
    {
        //TODO: Make this redundant, like with water and power.
        Earnings = 0;
        Goods = 0;
    }
    
    private void DistributeCitizens()
    {
        foreach (var tile in cityTiles)
        {
            if (tile.Value is not IResidence residential) continue;
            if (residential.Residents.Count == residential.MaxPopulation) continue;
            if (!tile.Value.IsActive) continue;

            var random = new Random();
            var residentsToAdd = random.Next(1, 5);
            residentsToAdd *= residential.GetPopulationMultiplier(cityTiles);

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
        foreach (var tile in cityTiles)
        {
            if (tile.Value is not IEmployer employer) continue;
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
        return Funds >= buildingPreset.CostToBuild;
    }

    public void NewTile(Vector3Int tilePosition, BuildingPreset buildingPreset)
    {
        Funds -= buildingPreset.CostToBuild;
        
        //TODO: Can this be moved into Awake() so we don't create a new building Factory class everytime?
        IBuildingFactory buildingFactory = new BuildingFactory();

        var buildable = buildingFactory.CreateBuilding(buildingPreset);

        //Need to replace if the tile already exists
        if (cityTiles.TryGetValue(tilePosition, out _))
        {
            cityTiles[tilePosition] = buildable;
        }
        else
        {
            cityTiles.Add(tilePosition, buildable);   
        }

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
}    