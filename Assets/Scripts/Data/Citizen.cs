using System;
using UnityEngine;

public class Citizen
{
    public Guid Id { get; private set; }
    public bool IsEmployed { get; set; }
    public Vector3Int HomeTile { get; set; }
    public Vector3Int WorkTile { get; set; }
    
    public Citizen(Vector3Int homeTile)
    {
        Id = Guid.NewGuid();
        HomeTile = homeTile;
        IsEmployed = false;
    }
}