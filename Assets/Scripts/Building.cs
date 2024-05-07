using UnityEngine;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;
    private BuildingPreset currentBuildingPreset;

    #region Build

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridManager.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //If the GridManager can take the area
        if (!GridManager.current.CanTakeArea(areaTemp))
        {
            return false;
        }
        //If the simulation manager allows for the cost to be subtracted from the treasury
        else if(SimulationManager.current.treasury < currentBuildingPreset.costToBuild)
        {
            return false;
        }
        //If all checks succeed return true;
        else
        {
            return true;
        }
    }

    public void Place()
    {
        Vector3Int positionInt = GridManager.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        Placed = true;

        GridManager.current.TakeArea(areaTemp);

        SimulationManager.current.OnPlaceBuilding(currentBuildingPreset);
    }

    #endregion
}
