using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tool", menuName = "Create Tool")]
public class BuildingTool : ScriptableObject
{
    [field: SerializeField] public string TileName { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public TileBase TileBase { get; private set; }
    [field: SerializeField] public PlacementType PlacementType { get; private set; }
}
