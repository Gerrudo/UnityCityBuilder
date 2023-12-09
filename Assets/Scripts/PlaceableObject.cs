using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool placed { get; private set; }
    private Vector3 origin;

    public BoundsInt area;

    public bool CanBePlaced()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return BuildingSystem.current.CanTakeArea(areaTemp);
    }

    public void Place()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        placed = true;

        BuildingSystem.current.TakeArea(areaTemp);
    }

    public void CheckPlacement()
    {
        if (CanBePlaced())
        {
            Place();
            origin = transform.position;
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }
}
