using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSettings")]
    public Texture2D mapTexture; //黑白地圖
    public bool flipY = true;

    public int mapWidth;
    public int mapHeight;

    public TileData[,] tileData; //儲存產生好的地圖資料


    private void Awake()
    {
        GenerateMapData();
    }

    public void GenerateMapData()
    {
        if(mapTexture == null)
        {
            Debug.LogError("沒放地圖");
            return;
        }

        //若未指定長寬就用圖片尺寸
        if(mapWidth ==0) mapWidth = mapTexture.width;
        if(mapHeight ==0) mapHeight = mapTexture.height;

        tileData = new TileData[mapWidth, mapHeight];

        //分析每個像素
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

        Debug.Log("地圖資料生成完畢");
    }
}
