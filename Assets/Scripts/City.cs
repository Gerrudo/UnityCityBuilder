using System.Collections;
using System.Collections.Generic;
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
        
        Population = 0;
        Earnings = 0;
        
        foreach (var building in cityTiles)
        {
            if (!CheckTileConnection(building.Key, TileType.Road)) continue;

            UpgradeBuilding(building.Key);
                
            building.Value.UpdateBuilding();

            Population += building.Value.Data.CurrentPopulation;
            Earnings += building.Value.Data.Taxes;
        }
        
        cityStatistics.UpdateUI();
        
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
        if (cityTiles[tilePosition].Data.TileType is TileType.Residential or TileType.Commercial or TileType.Industrial)
        {
            tileEditor.DrawItem(tilePosition, cityTiles[tilePosition].Data.Level1TilBase);
        }
    }
}    