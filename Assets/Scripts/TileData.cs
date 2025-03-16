using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public bool isLand; //不是陸地就是水

    public SetTownType setTownType = SetTownType.None; //預設 無聚落
}

public enum SetTownType
{
    None,
    City,
    Village,
    Industry
}

