public interface IBuildable
{
    BuildingData Data
    {
        get;
        set;
    }
    BuildingData NewBuildingData(GameTile gameTile);
}

public interface IBuildingFactory
{
    IBuildable CreateBuilding(TileType tileType);
}

public class BuildingData
{
    public TileType TileType { get; set; }
    public int PowerDemand { get; set; }
    public int WaterDemand { get; set; }
    public int CurrentPopulation { get; set; }
    public bool IsConnectedToRoad { get; set; }
    public bool IsPowered { get; set; }
    public bool IsWatered { get; set; }
    public int Happiness { get; set; }
    public int Taxes { get; set; }
    public int Expenses { get; set; }
    public int Workers { get; set; }
    public int MaxWorkers { get; set; }
}

public class Road : IBuildable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
            
        Data.Taxes = 10;
        
        return Data;
    }
}

public class Residential : IBuildable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        
        Data.Taxes = 10;
        
        return Data;
    }
}

public class Commercial : IBuildable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        
        Data.Taxes = 10;
        
        return Data;
    }
}


public class Industrial : IBuildable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        
        Data.Taxes = 10;
        
        return Data;
    }
}

public class Generator : IBuildable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        
        Data.Taxes = 10;
        
        return Data;
    }
}

public class WaterTower : IBuildable
{
    public BuildingData Data { get; set; }
    
    public BuildingData NewBuildingData(GameTile gameTile)
    {
        Data = new BuildingData();
        
        Data.Taxes = 10;
        
        return Data;
    }
}

public class BuildingFactory : IBuildingFactory
{
    public IBuildable CreateBuilding(TileType tileType)
    {
        return tileType switch
        {
            TileType.Road => new Road(),
            TileType.Residential => new Residential(),
            TileType.Commercial => new Commercial(),
            TileType.Industrial => new Industrial(),
            TileType.Generator => new Generator(),
            TileType.WaterTower => new WaterTower(),
            _ => throw new System.Exception("No TileType was provided.")
        };
    }
}