using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [Header("References")]
    public MapGenerator mapGenerator;
    public TilePool tilePool;

    [Header("Setting")]
    public float tileSize = 1.0f;

    List<GameObject> activeTiles = new List<GameObject>();
    Vector2 hexCoords;



    void Start()
    {
        MakeMap();
    }

    Vector2 GetHexCoords(int x, int y) // �վ㤻��Ϊ��s����m
    {
        float xPos = x * tileSize * Mathf.Cos(Mathf.Deg2Rad * 30);
        float yPos = y * tileSize + ((x % 2 == 1) ? tileSize * 0.5f : 0);

        return new Vector2(xPos, yPos);
    }


    public void MakeMap()
    {
        ClearTiles();

        var tileDatas = mapGenerator.tileData;
        if(tileDatas == null)
        {
            Debug.LogError("�S�I�s��a�ϸ�T");
            return;
        }

        int width = mapGenerator.mapWidth;
        int height = mapGenerator.mapHeight;

        //�ͦ�tile
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
               TileData data = tileDatas[x, y];

                //�q���������tile
                GameObject tileGO = tilePool.GetTile(data);
                if (tileGO != null) //�S��ƪ����L
                {
                    Debug.Log(data + "tile�ͦ�����");
                    continue;
                }
               

                tileGO.transform.SetParent(this.transform,false);
                hexCoords = GetHexCoords(x,y);
                tileGO.transform.localPosition = new Vector3(hexCoords.x, 0, hexCoords.y);
                activeTiles.Add(tileGO);

                if(data.tileObjectType == TileObjectType.FruitBush)
                {
                    GameObject fruitBush = tilePool.GetTileObject(TileObjectType.FruitBush);
                    fruitBush.transform.position = tileGO.transform.position +Vector3.up*0.08f;
                    activeTiles.Add(fruitBush);
                }else if (data.tileObjectType == TileObjectType.InsectGrass)
                {
                    GameObject grass = tilePool.GetTileObject(TileObjectType.InsectGrass);
                    grass.transform.position = tileGO.transform.position + Vector3.up * 0.08f;
                    activeTiles.Add(grass);
                }
            }
        }
    }

    public void ClearTiles()
    {
        foreach (var tileGo in activeTiles)
        {
            TileData tileData = tileGo.GetComponent<TileData>();

            if (tileData == null)
            {
                Debug.Log(tileGo.name + "�S��TileData");
                continue;
            }

            tilePool.ReturnTile(tileGo, tileData);

            if(tileData.tileObjectType == TileObjectType.FruitBush)
            {
                tilePool.ReturnObject(tileGo,TileObjectType.FruitBush);
            }else if (tileData.tileObjectType == TileObjectType.InsectGrass)
            {
                tilePool.ReturnObject(tileGo,TileObjectType.InsectGrass);
            }
        }

        activeTiles.Clear();
       
    }

}
    
      
    
