using System.Collections.Generic;
using UnityEngine;

public interface IResidence
{
    int MaxPopulation { get; set; }
    List<Citizen> Residents { get; set; }
    int GetPopulationMultiplier(IReadOnlyDictionary<Vector3Int, Building> cityTiles);
}