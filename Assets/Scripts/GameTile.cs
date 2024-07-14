using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "New Game Tile")]
public class GameTile : ScriptableObject
{
    [field: SerializeField] public TileType TileType { get; private set; }
    [field: SerializeField] public TileBase TileBase { get; private set; }
    [field: SerializeField] public TileBase Level1TilBase { get; private set; }
    [field: SerializeField] public int CostToBuild { get; private set; }
    [field: SerializeField] public int MaxPopulation { get; private set; }
    [field: SerializeField] public int PowerProduction { get; private set; }
    [field: SerializeField] public int WaterProduction { get; private set; }
    [field: SerializeField] public int Expenses { get; private set; }
}