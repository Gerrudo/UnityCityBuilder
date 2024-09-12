using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;

public class City : Singleton<City>
{
    public Dictionary<Vector3Int, Building> CityTiles { get; private set; }

    private BuildingFactory buildingFactory;

    private Timer updateTimer;
    private Timer dayTimer;

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
    public double ApprovalRating { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        CityTiles = new Dictionary<Vector3Int, Building>();
        
        buildingFactory = new BuildingFactory();
    }

    private void Start()
    {
        updateTimer = new Timer(CountDays, null, 0, SecondsPerDay);
        dayTimer = new Timer(UpdateCity, null, 0, SecondsPerUpdate);
    }

    private void CountDays(object state)
    {
        //Remember, this is not on main thread!
        
        Day++;

        Funds += Earnings;
    }
    
    private void UpdateCity(object state)
    {
        //Remember, this is not on main thread!
        
        foreach (var building in CityTiles.ToArray())
        {
            ProcessBuilding(building.Key, building.Value);
        }
        
        Earnings = CityTiles.Values.Sum(building => building.Taxes);
        Power = CityTiles.Values.Sum(building => building.Power);
        Water = CityTiles.Values.Sum(building => building.Power);
        Population = CityTiles.Values.Sum(building => building.Population);
    }

    private void ProcessBuilding(Vector3Int tilePosition, Building building)
    {
        building.IsConnectedToRoad = TilemapExtension.CheckTileConnection(tilePosition, TileType.Road, CityTiles);

        building.Update();
    }

    public bool CanPlaceNewTile(BuildingPreset buildingPreset)
    {
        return Funds >= buildingPreset.CostToBuild;
    }

    public void NewTile(Vector3Int tilePosition, BuildingPreset buildingPreset)
    {
        Funds -= buildingPreset.CostToBuild;

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
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        CityTiles.Remove(tilePosition);
    }

    private void OnDisable()
    {
        updateTimer?.Dispose();
        dayTimer?.Dispose();
    }
}    