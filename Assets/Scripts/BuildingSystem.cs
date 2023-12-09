using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap mainTileMap;
    public TileBase takenTile;

    private PlaceableObject temp;
    private Vector3 prevPos;

    #region Unity Methods

    void Awake()
    {
        current = this;
    }

    #endregion

    #region Building Placement

    public void InitialiseWithBuilding(GameObject building)
    {
        temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<PlaceableObject>();
    }

    public void InitialiseWithObject(GameObject building, Vector3 pos)
    {
        pos.z = 0;
        pos.y -= building.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        //Interpolation allows for building objects to snap to the tilemap
        Vector3 postion = gridLayout.CellToLocalInterpolated(cellPos);

        GameObject obj = Instantiate(building, postion, Quaternion.identity);
        PlaceableObject temp = obj.transform.GetComponent<PlaceableObject>();
        temp.gameObject.AddComponent<ObjectDrag>();
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, mainTileMap);

        foreach (var b in baseArray)
        {
            if (b == takenTile)
            {
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, takenTile, mainTileMap);
    }

    #endregion

    #region Tilemap Management

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tileMap)
    {
        TileBase[] array = new TileBase[area.size.x ^ area.size.y];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tileMap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileBase tileBase, Tilemap tileMap)
    {
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y];
        FillTiles(tileArray, tileBase);
        tileMap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileBase tileBase)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBase;
        }
    }

    public void ClearArea(BoundsInt area, Tilemap tileMap)
    {
        SetTilesBlock(area, null, tileMap);
    }

    #endregion
}
