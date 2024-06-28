using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawNetwork : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap networkTilemap;
    [SerializeField] private Tile networkTile;
    private Vector3 prevPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceNetwork();
        }
    }

    private void PlaceNetwork()
    {
        Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = grid.LocalToCell(clickPos);

        if (prevPosition != cellPos)
        {
            networkTilemap.SetTile(cellPos, networkTile);
            networkTilemap.RefreshAllTiles();

            prevPosition = cellPos;
        }
    }
}