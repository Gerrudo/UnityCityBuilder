using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public static GridManager current;

    public GridLayout gridLayout;
    public Tilemap placementTileMap;
    public Tilemap resourceTileMap;
    public Tilemap roadMap;
    public Tile roadTile;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();
    private Building tempBuilding;
    private Vector3 prevPosition;
    private bool drawingRoad = false;

    #region Unity

    private void Awake()
    {
        current = this;

        ShowTileMap(placementTileMap, false);
    }

    void Start()
    {
        string tilePath = "Tiles/";

        //Placement Indication
        tileBases.Add(TileType.Available, Resources.Load<TileBase>(tilePath + "AvailableTile"));
        tileBases.Add(TileType.Reserved, Resources.Load<TileBase>(tilePath + "ReservedTile"));
        tileBases.Add(TileType.Unavailable, Resources.Load<TileBase>(tilePath + "UnavailableTile"));

        //Resources
        tileBases.Add(TileType.Clay, Resources.Load<TileBase>(tilePath + "ClayTile"));
        tileBases.Add(TileType.Coal, Resources.Load<TileBase>(tilePath + "CoalTile"));
    }

    void Update()
    {
        if (!tempBuilding)
        {
            return;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject(0))
                {
                    return;
                }

                if (!tempBuilding.Placed)
                {
                    Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int cellPos = gridLayout.LocalToCell(clickPos);

                    if (prevPosition != cellPos)
                    {
                        tempBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(0f, 0f, 0f));
                        prevPosition = cellPos;

                        FollowBuilding();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (tempBuilding.CanBePlaced())
                {
                    tempBuilding.Place();
                    ShowTileMap(placementTileMap, false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(tempBuilding.gameObject);
                ShowTileMap(placementTileMap, false);
            }
        }

        if (drawingRoad)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreateRoad();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CreateRoad(false);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                drawingRoad = false;
                ShowTileMap(placementTileMap, false);
            }
        }
    }

    #endregion

    #region TileMap

    public enum TileType
    {
        Available,
        Reserved,
        Unavailable,
        Clay,
        Coal
    }

    #endregion

    #region Placement

    private void ShowTileMap(Tilemap tileMap, bool isVisable)
    {
        if (isVisable)
        {
            Debug.Log("Disabling Tilemap...");
        }
        else if (!isVisable)
        {
            Debug.Log("Disabling Tilemap...");
        }

        tileMap.GetComponent<TilemapRenderer>().enabled = isVisable;
    }

    public void InitialiseWithBuilding(BuildingPreset buildingPreset)
    {
        ShowTileMap(placementTileMap, true);

        tempBuilding = Instantiate(buildingPreset.prefab, Vector3.zero, Quaternion.identity).GetComponent<Building>();

        //This is required for the Simulation Manager class to access the values from the BuildingPreset via the Building class.
        tempBuilding.currentBuildingPreset = buildingPreset;

        FollowBuilding();
    }

    public void StartDrawingRoad()
    {
        ShowTileMap(placementTileMap, true);

        drawingRoad = true;
    }

    private void CreateRoad(bool erase = false)
    {
        Vector3Int hoveredCellPostion = roadMap.WorldToCell(Camera.main.ScreenToViewportPoint(Input.mousePosition));

        hoveredCellPostion = new Vector3Int(hoveredCellPostion.x, hoveredCellPostion.y, 0);

        roadMap.SetTile(hoveredCellPostion, erase ? null : roadTile);
        roadMap.RefreshAllTiles();
    }

    private void FollowBuilding()
    {
        tempBuilding.area.position = gridLayout.WorldToCell(tempBuilding.gameObject.transform.position);
        BoundsInt buildingArea = tempBuilding.area;

        TileBase[] baseArray = placementTileMap.GetTilesBlock(buildingArea);

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.Available])
            {
                Debug.Log("Building placement allowed here.");
            }
        }
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = placementTileMap.GetTilesBlock(area);

        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.Available])
            {
                string statusMessage = "Building cannot be placed here.";

                Debug.Log(statusMessage);
                StatusMessage.current.UpdateStatusMessage(statusMessage);

                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y * area.size.z];

        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i] = tileBases[TileType.Reserved];
        }

        placementTileMap.SetTilesBlock(area, tileArray);
    }

    public bool IsOnResource(BoundsInt area, TileType resourceTile)
    {
        TileBase[] baseArray = resourceTileMap.GetTilesBlock(area);

        foreach (var b in baseArray)
        {
            if (b != tileBases[resourceTile])
            {
                string statusMessage = "Building must be placed on a resource tile.";

                Debug.Log(statusMessage);
                StatusMessage.current.UpdateStatusMessage(statusMessage);

                return false;
            }
        }

        return true;
    }

    #endregion
}