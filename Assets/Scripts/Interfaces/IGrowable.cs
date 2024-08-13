using UnityEngine.Tilemaps;

public interface IGrowable
{
    TileBase TileBase { get; set; }
    TileBase Level1TilBase { get; set; }
    bool CanUpgrade();
}
