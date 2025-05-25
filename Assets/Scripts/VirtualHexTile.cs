using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualHexTile 
{
    public Vector2Int gridPos;
    public TileData tileData;
    public GameObject tileObject;


    public bool HasGameObject => tileObject != null; //快速判斷該格子有沒有對應的 GameObject

    public VirtualHexTile(Vector2Int pos,TileData data)
    {
        gridPos = pos;
        tileData = data;
    }
}
