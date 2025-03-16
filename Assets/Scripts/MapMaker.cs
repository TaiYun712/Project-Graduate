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

                tileGO.transform.SetParent(this.transform,false);

                hexCoords = GetHexCoords(x,y);
                tileGO.transform.localPosition = new Vector3(hexCoords.x, 0, hexCoords.y);

                activeTiles.Add(tileGO);
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

            if (tileGo.CompareTag("LandTile"))
            {
                tilePool.ReturnTile(tileGo, tileData);
            }else if (tileGo.CompareTag("WaterTile"))
            {
                tilePool.ReturnTile(tileGo, tileData);
            }else if (tileGo.CompareTag("CityTile"))
            {
                tilePool.ReturnTile(tileGo, tileData);
            }else if (tileGo.CompareTag("VillageTile"))
            {
                tilePool.ReturnTile(tileGo, tileData);
            }else if (tileGo.CompareTag("IndustryTile"))
            {
                tilePool.ReturnTile(tileGo, tileData);
            }
            else
            {
                tilePool.ReturnTile(tileGo, tileData);
            }
        }

        activeTiles.Clear();
    }

}
    
      
    
