using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public bool isLand; //���O���a�N�O��

    public SetTownType setTownType = SetTownType.None; //�w�] �L�E��
}

public enum SetTownType
{
    None,
    City,
    Village,
    Industry
}

