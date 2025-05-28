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


    VirtualHexGrid hexGrid = new VirtualHexGrid();
    public VirtualHexGrid HexGrid => hexGrid;


    void Start()
    {
        MakeMap();
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

               Vector2Int pos = new Vector2Int(x, y);
               TileData data = mapData[x, y];

               if(data == null)
                {
                    data = new TileData { isLand = false};
                }

                hexGrid.SetTile(pos,data);

                if (mapGenerator.IsGray(x, y)) { continue; } //���ͦ��Ǧ��l

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
                

                var behavior = tileGO.GetComponent<TileBehaviour>();
                if( behavior != null)
                {
                    behavior.gridPos = pos;
                    behavior.tileData = data;
                }

                hexGrid.GetTile(pos).tileObject = tileGO;
                activeTiles.Add(tileGO);
                Debug.Log($"�{�b�@��{activeTiles.Count}��tile");
            }
        }
    }

    public void ClearTiles()
    {
        foreach (var tileGo in activeTiles)
        {
            var behavior = tileGo.GetComponent<TileBehaviour>();
            if (behavior == null || behavior.tileData == null) {  continue; }

            tilePool.ReturnTile(tileGo, behavior.tileData);
        }

        activeTiles.Clear();
       
    }

    Vector2 GetHexCoords(int x, int y) // �վ㤻��Ϊ��s����m
    {
        
        float xPos = x * tileSize * Mathf.Cos(Mathf.Deg2Rad * 30);
        float yPos = y * tileSize + ((x % 2 == 1) ? tileSize * 0.5f : 0);

        return new Vector2(xPos, yPos);
        
       // return hexGrid.GridToWorld(new Vector2Int(x, y), tileSize);
    }

}
    
      
    
