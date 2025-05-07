using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSettings")]
    public Texture2D mapTexture; //地圖

    public int mapWidth;
    public int mapHeight;

    public int colorTolerance = 10; //顏色偏差值

    [Header("TownSetting")]
    [SerializeField] int allTownPlace;  
    public float cityRate;
    public float villageRate;

    //顏色
    private static readonly Color32 BLACK = new Color32(0, 0, 0, 255);       //土地
    private static readonly Color32 WHITE = new Color32(255, 255, 255, 255); //水
    private static readonly Color32 GRAY = new Color32(167, 167, 167, 255);  //無
    private static readonly Color32 ORANGE = new Color32(255, 180, 0, 255);  //聚落
   

    public TileData[,] GenerateMapData()
    {
         mapWidth = mapTexture.width;
         mapHeight = mapTexture.height;
        TileData[,] mapData = new TileData[mapWidth, mapHeight];


        //分析每個像素
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Color32 px = mapTexture.GetPixel(x, y);

                if (IsColorClose(px, GRAY,colorTolerance)) { continue; }

                TileData data = new TileData();
                if (IsColorClose(px, BLACK, colorTolerance))
                {
                    data.isLand = true;
                }
                else if(IsColorClose(px,WHITE, colorTolerance))
                {
                    data.isLand = false;
                }
                else if(IsColorClose(px, ORANGE, colorTolerance))
                {
                    data.setTownType = GetRandomTownType();
                }
                else
                {
                    Debug.Log($"未辨識顏色:{px} at ({x},{y})");
                    continue;
                }

                mapData[x, y] = data;
            }
        }

        return mapData;
    }

    
    bool IsColorClose(Color32 a,Color32 b,int tolerance)
    {
        return Mathf.Abs(a.r - b.r) <= tolerance &&
               Mathf.Abs(a.g - b.g) <= tolerance &&
                Mathf.Abs(a.b - b.b) <= tolerance;
    }
    
   

    //隨機聚落類型
    SetTownType GetRandomTownType()
    {
        float rand = Random.value;
        if (rand < cityRate) { return SetTownType.City; }
        if (rand < cityRate + villageRate) { return SetTownType.Village; } 
        return SetTownType.Industry;
    }
}
