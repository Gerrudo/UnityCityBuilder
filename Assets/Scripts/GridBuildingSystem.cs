using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap placementTileMap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building tempBuilding;
    private Vector3 prevPosition;

    private static int CalculateArea(BoundsInt area)
    {
        return area.size.x * area.size.y * area.size.z;
    }

    #region Unity

    private void Awake()
    {
        current = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        string tilePath = "Tiles/";

        tileBases.Add(TileType.Available, Resources.Load<TileBase>(tilePath + "AvailableTile"));
        tileBases.Add(TileType.Reserved, Resources.Load<TileBase>(tilePath + "ReservedTile"));
        tileBases.Add(TileType.Unavailable, Resources.Load<TileBase>(tilePath + "UnavailableTile"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!tempBuilding)
        {
            return;
        }

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
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(tempBuilding.gameObject);
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

    private static void FillTiles(TileBase[] array, TileType type)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = tileBases[type];
        }
    }

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        TileBase[] tileArray = new TileBase[CalculateArea(area)];

        FillTiles(tileArray, type);

        tilemap.SetTilesBlock(area, tileArray);
    }

    #endregion

    #region Placement

    public void InitialiseWithBuilding(GameObject building)
    {
        tempBuilding = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();

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
                Debug.Log("Building cannot be placed here.");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Reserved, placementTileMap);
    }

    #endregion
}
