using UnityEngine;
using UnityEngine.Tilemaps;

public class NetworkTile : Tile
{
    public networkTileRule[] rules;

    public Sprite defaultSprite;

    [System.Serializable]
    public class networkTileRule
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

        tileData.sprite = fittingSprite;
    }

    private static int ConditionCheck(TileBase tile, bool condition)
    {
        if (condition)
        {
            if (tile is NetworkTile)
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
            if (tile is NetworkTile)
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