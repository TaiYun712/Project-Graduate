using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public bool isLand; //���O���a�N�O���A�w�]�����a

    public SetTownType setTownType = SetTownType.None; //�w�] �L�E��

    public TileObjectType tileObjectType  = TileObjectType.None; //��������
}

public enum SetTownType
{
    None,
    City,
    Village,
    Industry
}

public enum TileObjectType
{
    None,
    FruitBush,
    InsectGrass
}

