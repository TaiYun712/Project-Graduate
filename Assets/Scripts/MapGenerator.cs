using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSettings")]
    public Texture2D mapTexture; //�a��

    public int mapWidth;
    public int mapHeight;

    public int colorTolerance = 10; //�C�ⰾ�t��

    [Header("TownSetting")]
    [SerializeField] int allTownPlace;  
    public float cityRate;
    public float villageRate;

    //�C��
    private static readonly Color32 BLACK = new Color32(0, 0, 0, 255);       //�g�a
    private static readonly Color32 WHITE = new Color32(255, 255, 255, 255); //��
    private static readonly Color32 GRAY = new Color32(167, 167, 167, 255);  //�L
    private static readonly Color32 ORANGE = new Color32(255, 180, 0, 255);  //�E��
   

    public TileData[,] GenerateMapData()
    {
         mapWidth = mapTexture.width;
         mapHeight = mapTexture.height;
        TileData[,] mapData = new TileData[mapWidth, mapHeight];


        //���R�C�ӹ���
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
                    Debug.Log($"�������C��:{px} at ({x},{y})");
                    continue;
                }

                mapData[x, y] = data;
            }
        }

        return mapData;
    }

    //�C�ⰾ�t�e�\
    bool IsColorClose(Color32 a,Color32 b,int tolerance)
    {
        return Mathf.Abs(a.r - b.r) <= tolerance &&
               Mathf.Abs(a.g - b.g) <= tolerance &&
                Mathf.Abs(a.b - b.b) <= tolerance;
    }

    //�Ѩ�L�}���P�_�Ǧ⤣�ͦ��ϡA�æb�u����ͦ����q�v�ɨM�w�����ͪ���
    public bool IsGray(int x,int y)
    {
        if(x < 0 || x >= mapWidth || y < 0 || y >= mapHeight) {  return false; }
        return IsColorClose(mapTexture.GetPixel(x, y), GRAY, colorTolerance);
    }

    //�H���E������
    SetTownType GetRandomTownType()
    {
        float rand = Random.value;
        if (rand < cityRate) { return SetTownType.City; }
        if (rand < cityRate + villageRate) { return SetTownType.Village; } 
        return SetTownType.Industry;
    }
}
