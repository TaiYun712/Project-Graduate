using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public bool isLand; //���O���a�N�O���A�w�]�����a

    public SetTownType setTownType = SetTownType.None; //�w�] �L�E��

    
}

public enum SetTownType
{
    None,
    City,
    Village,
    Industry
}



