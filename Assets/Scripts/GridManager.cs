using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public static GridManager current;

    public GridLayout gridLayout;
    public Tilemap placementTileMap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building tempBuilding;

    private Vector3 prevPosition;

    #region Unity

    private void Awake()
    {
        current = this;

        ShowTileMap(placementTileMap, false);
    }

    void Start()
    {
        string tilePath = "Tiles/";

        tileBases.Add(TileType.Available, Resources.Load<TileBase>(tilePath + "AvailableTile"));
        tileBases.Add(TileType.Reserved, Resources.Load<TileBase>(tilePath + "ReservedTile"));
        tileBases.Add(TileType.Unavailable, Resources.Load<TileBase>(tilePath + "UnavailableTile"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!placementTileMap.GetComponent<TilemapRenderer>().enabled)
            {
                ShowTileMap(placementTileMap, true);
            }
            else
            {
                ShowTileMap(placementTileMap, false);
            }
        }

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
    }

    #endregion

    #region TileMap

    public enum TileType
    {
        Available,
        Reserved,
        Unavailable
    }

    #endregion

    #region Placement

    public void ShowTileMap(Tilemap tileMap, bool isVisable)
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

    #endregion
}