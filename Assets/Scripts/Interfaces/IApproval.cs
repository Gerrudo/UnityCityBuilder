using System.Collections.Generic;
using UnityEngine;

public interface IApproval
{
    double GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles);
}