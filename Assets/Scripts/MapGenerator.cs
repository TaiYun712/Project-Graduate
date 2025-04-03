using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSettings")]
    public Texture2D mapTexture; //�a��
    public bool flipY = true;

    public int mapWidth;
    public int mapHeight;

    public TileData[,] tileData; //�x�s���ͦn���a�ϸ��

    public int colorTolerance = 10; //�C�ⰾ�t��

    [Header("TownSetting")]
    [SerializeField] int allTownPlace;  
    public float cityRate;
    public float villageRate;

    //�C��
    private static readonly Color32 BLACK = new Color32(0, 0, 0, 255);
    private static readonly Color32 WHITE = new Color32(255, 255, 255, 255);
    private static readonly Color32 GRAY = new Color32(167, 167, 167, 255);
    private static readonly Color32 ORANGE = new Color32(255, 180, 0, 255);
    private static readonly Color32 YELLOW = new Color32(255, 253, 47, 255);
    private static readonly Color32 RED = new Color32(255, 14, 14, 255);
    private static readonly Color32 BROWN = new Color32(112, 79, 6, 255);
    private static readonly Color32 BLUE = new Color32(18, 196, 255, 255);
    private static readonly Color32 GREEN = new Color32(83, 255,25, 255);


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
        List<Vector2Int> townCenters = new List<Vector2Int>(); //�s��i�ͦ��E�����ߪ���m
        List<Vector2Int> townAreas = new List<Vector2Int>(); //�s��i�ͦ��E���}�o�檺��m
        List<Vector2Int> fruitPlaces = new List<Vector2Int>();//�s��i�ͦ��G�O����m
        List<Vector2Int> insectPlaces = new List<Vector2Int>();//�s��i�ͦ����ί��O����m

        //���R�C�ӹ���
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Color32 pxColor = flipY ?
               (Color32) mapTexture.GetPixel(x, mapTexture.height - 1 - y): 
               (Color32) mapTexture.GetPixel(x, y);
               // Debug.Log(pxColor);

                bool isLand = false; //�a��
                SetTownType setTownType = SetTownType.None; //�E��
                TileObjectType tileObject = TileObjectType.None; //����

                if (IsColorClose(pxColor,BLACK,colorTolerance)) { isLand = true; } //�ͦ��g�a-��
                else if (IsColorClose(pxColor, WHITE, colorTolerance)) { isLand = false; }//�ͦ���-��
                else if (IsColorClose(pxColor, GRAY, colorTolerance)) { continue; } //���ͦ�-��
                else if (IsColorClose(pxColor, ORANGE, colorTolerance)) //�ͦ��E������-��
                { isLand = true; townCenters.Add(new Vector2Int(x, y)); }
                else if (IsColorClose(pxColor,YELLOW, colorTolerance)) //�ͦ��E���}�o�ϰ�-��
                { isLand = true;townAreas.Add(new Vector2Int(x, y)); }
                else if (IsColorClose(pxColor,RED, colorTolerance)) //�ͦ��G�O-��
                { isLand = true;tileObject = TileObjectType.FruitBush;fruitPlaces.Add(new Vector2Int(x, y)); }
                else if(IsColorClose(pxColor, BROWN, colorTolerance))  //�ͦ����O-��
                {  isLand = true; tileObject = TileObjectType.InsectGrass;insectPlaces.Add(new Vector2Int(x, y)); }
                else if(IsColorClose(pxColor, BLUE, colorTolerance)) {  isLand = true;} //�_�I
                else if (IsColorClose(pxColor, GREEN, colorTolerance)) {  isLand = true;} //���I
                else
                {
                    Debug.Log($"�y��: ({x}, {y})��TileData �� null�I");                  
                }

                tileData[x, y] = new TileData { isLand = isLand, setTownType = setTownType,tileObjectType = tileObject };

               
            }
        }

        allTownPlace = townCenters.Count;
        Debug.Log($"���{allTownPlace}�ӻE���ͦ��I,{ townAreas.Count}��}�o�ϰ�");

        AssignTowns(townCenters,townAreas);
    }

    
    bool IsColorClose(Color32 a,Color32 b,int tolerance)
    {
        return Mathf.Abs(a.r - b.r) <= tolerance &&
               Mathf.Abs(a.g - b.g) <= tolerance &&
                Mathf.Abs(a.b - b.b) <= tolerance;
    }
    
    void AssignTowns(List<Vector2Int> townCenters,List<Vector2Int> townAreas)
    {
        if (townCenters.Count == 0) return;
        Debug.Log($"�@��{townCenters.Count}�ӻE��");

        foreach (var center in townCenters)
        {
            SetTownType townType = GetRandomTownType();
            tileData[center.x, center.y].setTownType = townType;

            foreach (var area in townAreas)
            {
                if (IsNeighbor(center, area))
                {
                    tileData[area.x,area.y].setTownType = townType;
                }
            }
        }

        Debug.Log($"�w���t{townCenters.Count}�ӻE��");
       
    }

    //�P�_��l�O�_�۾F
    bool IsNeighbor(Vector2Int a,Vector2Int b) 
    {
        Vector2Int[] directions = GetHexDirections();
        foreach (var dir in directions)
        {
            if(a + dir == b)return true;
        }
        return false;
    }

    //����ή�l�۾F��V
    Vector2Int[] GetHexDirections()
    {
        return new Vector2Int[]
        {
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, -1), new Vector2Int(-1, 1)
        };
    }

    //�H���E������
    SetTownType GetRandomTownType()
    {
        float rand = Random.value;
        if (rand < cityRate) return SetTownType.City;
        if (rand < cityRate + villageRate) return SetTownType.Village;
        return SetTownType.Industry;
    }
}
