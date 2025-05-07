using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public TileData tileData;

    public Vector2Int gridPos;

    public bool IsLand
    {
        get
        {
            return tileData != null ? tileData.isLand : false;
        }
    }

    public bool HasTown
    {
        get
        {
            return tileData != null ? tileData.setTownType != SetTownType.None : false;
        }
    }
}
