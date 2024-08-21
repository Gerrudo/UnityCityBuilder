using System.Collections.Generic;
using UnityEngine;

public interface IApprovable
{
    double GetApprovalScore(IReadOnlyDictionary<Vector3Int, Building> cityTiles);
}