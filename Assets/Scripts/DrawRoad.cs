using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawRoad : MonoBehaviour
{
    public Grid grid;
    public Tilemap roadMap;
    public Tile roadTile;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateRoad();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            CreateRoad(false);
        }
    }

    private void CreateRoad(bool erase=false)
    {
        Vector3Int hoveredCellPostion = roadMap.WorldToCell(Camera.main.ScreenToViewportPoint(Input.mousePosition));

        hoveredCellPostion = new Vector3Int(hoveredCellPostion.x, hoveredCellPostion.y, 0);

        roadMap.SetTile(hoveredCellPostion, erase ? null : roadTile);
        roadMap.RefreshAllTiles();
    }
}
