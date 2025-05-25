using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualHexTile 
{
    public Vector2Int gridPos;
    public TileData tileData;
    public GameObject tileObject;


    public bool HasGameObject => tileObject != null; //�ֳt�P�_�Ӯ�l���S�������� GameObject

    public VirtualHexTile(Vector2Int pos,TileData data)
    {
        gridPos = pos;
        tileData = data;
    }
}
