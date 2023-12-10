using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap placementTileMap;
    public Tilemap indicatorTileMap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    private Building tempBuilding;
    private Vector3 prevPosition;
    private BoundsInt prevArea;

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
        string tilePath = "Assets/Tiles/";

        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "WhiteTile"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "GreenTile"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "RedTile"));
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
                    tempBuilding.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
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
            ClearArea();
            Destroy(tempBuilding.gameObject);
        }
    }

    #endregion

    #region TileMap

    public enum TileType
    {
        Empty,
        White,
        Green,
        Red
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[CalculateArea(area)];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);

            counter++;
        }

        return array;
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

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[CalculateArea(prevArea)];

        FillTiles(toClear, TileType.Empty);

        indicatorTileMap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        tempBuilding.area.position = gridLayout.WorldToCell(tempBuilding.gameObject.transform.position);
        BoundsInt buildingArea = tempBuilding.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, placementTileMap);
        TileBase[] tileArray = new TileBase[baseArray.Length];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        placementTileMap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, placementTileMap);

        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, indicatorTileMap);
        SetTilesBlock(area, TileType.Green, placementTileMap);
    }

    #endregion
}
