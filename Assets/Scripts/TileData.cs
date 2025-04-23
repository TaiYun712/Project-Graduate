using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public bool isLand; //不是陸地就是水，預設為陸地

    public SetTownType setTownType = SetTownType.None; //預設 無聚落

    
}

public enum SetTownType
{
    None,
    City,
    Village,
    Industry
}



