using UnityEngine;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;
    public BuildingPreset currentBuildingPreset;

    #region Build

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridManager.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (!GridManager.current.CanTakeArea(areaTemp))
        {
            return false;
        }
        else if (SimulationManager.current.bricks < currentBuildingPreset.bricksToBuild)
        {
            string statusMessage = "Not enough bricks to place building.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return false;
        }
        else if (SimulationManager.current.money < currentBuildingPreset.moneyToBuild)
        {
            string statusMessage = "Not enough money to place building.";

            Debug.Log(statusMessage);
            StatusMessage.current.UpdateStatusMessage(statusMessage);

            return false;
        }
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
