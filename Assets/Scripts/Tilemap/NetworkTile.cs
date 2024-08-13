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

        var fittingSprite = defaultSprite;

        var n = tilemap.GetTile(position.North());
        var e = tilemap.GetTile(position.East());
        var s = tilemap.GetTile(position.South());
        var w = tilemap.GetTile(position.West());

        foreach (var rule in rules)
        {
            var conditionsMet = 0;

            conditionsMet += ConditionCheck(n, rule.northConnected);
            conditionsMet += ConditionCheck(e, rule.eastConnected);
            conditionsMet += ConditionCheck(s, rule.southConnected);
            conditionsMet += ConditionCheck(w, rule.westConnected);

            if (conditionsMet != 4) continue;
            
            fittingSprite = rule.sprite;
            
            break;
        }

        tileData.sprite = fittingSprite;
    }

    private static int ConditionCheck(TileBase tile, bool condition)
    {
        if (condition) return tile is NetworkTile ? 1 : 0;

        return tile is NetworkTile ? 0 : 1;
    }
}