using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSettings")]
    public Texture2D mapTexture; //�¥զa��
    public bool flipY = true;

    public int mapWidth;
    public int mapHeight;

    public TileData[,] tileData; //�x�s���ͦn���a�ϸ��


    private void Awake()
    {
        GenerateMapData();
    }

    public void GenerateMapData()
    {
        if(mapTexture == null)
        {
            Debug.LogError("�S��a��");
            return;
        }

        //�Y�����w���e�N�ιϤ��ؤo
        if(mapWidth ==0) mapWidth = mapTexture.width;
        if(mapHeight ==0) mapHeight = mapTexture.height;

        tileData = new TileData[mapWidth, mapHeight];

        //���R�C�ӹ���
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float pxGray = flipY ?
                mapTexture.GetPixel(x, mapTexture.height - 1 - y).grayscale : 
                mapTexture.GetPixel(x, y).grayscale;

                bool isLand = (pxGray < 0.5f);

                tileData[x, y] = new TileData { isLand =  isLand };
            }
        }

        Debug.Log("�a�ϸ�ƥͦ�����");
    }
}
