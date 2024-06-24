using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadTile : Tile
{
    public roadRule[] rules;

    public Sprite defaultSprite;

    [System.Serializable]
    public class roadRule
    {
        public bool northConnected;
        public bool eastConnected;
        public bool southConnected;
        public bool westConnected;

        public Sprite sprite;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        Sprite fittingSprite = defaultSprite;

        TileBase n = tilemap.GetTile(position.North());
        TileBase e = tilemap.GetTile(position.East());
        TileBase s = tilemap.GetTile(position.South());
        TileBase w = tilemap.GetTile(position.West());

        foreach (var rule in rules)
        {
            int conditionsMet = 0;

            conditionsMet += ConditionCheck(n, rule.northConnected);
            conditionsMet += ConditionCheck(e, rule.eastConnected);
            conditionsMet += ConditionCheck(s, rule.southConnected);
            conditionsMet += ConditionCheck(w, rule.westConnected);

            if (conditionsMet == 4)
            {
                fittingSprite = rule.sprite;
                break;
            }
        }
    }

    private static int ConditionCheck(TileBase tile, bool condition)
    {
        if (condition)
        {
            if (tile is RoadTile)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if (tile is RoadTile)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}