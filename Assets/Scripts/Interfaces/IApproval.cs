using System.Collections.Generic;
using UnityEngine;

public interface IApproval
{
    float GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles);
}