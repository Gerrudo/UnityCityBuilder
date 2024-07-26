using System;
using UnityEngine;

public class Citizen
{
    public bool IsEmployed { get; set; }
    public Vector3Int HomeTile { get; set; }
    public Vector3Int WorkTile { get; set; }
    
    public Citizen(Vector3Int homeTile)
    {
        HomeTile = homeTile;
        IsEmployed = false;
    }
}