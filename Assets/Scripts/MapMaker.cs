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

        TileData[,] mapData = mapGenerator.GenerateMapData();
        int width = mapData.GetLength(0);
        int height = mapData.GetLength(1);

        if (mapData == null)
        {
            Debug.LogError("�S�I�s��a�ϸ�T");
            return;
        }

            
       //�ͦ�tile
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
               TileData data = mapData[x, y];
               if(data == null) { continue; }

                //�q���������tile
                GameObject tileGO = tilePool.GetTile(data);
                if (tileGO == null) //�S��ƪ����L
                {
                    Debug.Log(data + "tile�ͦ�����");
                    continue;
                }
               

                tileGO.transform.SetParent(this.transform,false);
                hexCoords = GetHexCoords(x,y);
                tileGO.transform.localPosition = new Vector3(hexCoords.x, 0, hexCoords.y);
                activeTiles.Add(tileGO);

                var behavior = tileGO.GetComponent<TileBehaviour>();
                if( behavior != null)
                {
                    behavior.gridPos = new Vector2Int(x, y);
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

           
        }

        activeTiles.Clear();
       
    }

}
    
      
    
