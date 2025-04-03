using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSettings")]
    public Texture2D mapTexture; //地圖
    public bool flipY = true;

    public int mapWidth;
    public int mapHeight;

    public TileData[,] tileData; //儲存產生好的地圖資料

    public int colorTolerance = 10; //顏色偏差值

    [Header("TownSetting")]
    [SerializeField] int allTownPlace;  
    public float cityRate;
    public float villageRate;

    //顏色
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
            Debug.LogError("沒放地圖");
            return;
        }

        //若未指定長寬就用圖片尺寸
        if(mapWidth ==0) mapWidth = mapTexture.width;
        if(mapHeight ==0) mapHeight = mapTexture.height;


        tileData = new TileData[mapWidth, mapHeight];
        List<Vector2Int> townCenters = new List<Vector2Int>(); //存放可生成聚落中心的位置
        List<Vector2Int> townAreas = new List<Vector2Int>(); //存放可生成聚落開發格的位置
        List<Vector2Int> fruitPlaces = new List<Vector2Int>();//存放可生成果叢的位置
        List<Vector2Int> insectPlaces = new List<Vector2Int>();//存放可生成昆蟲草叢的位置

        //分析每個像素
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Color32 pxColor = flipY ?
               (Color32) mapTexture.GetPixel(x, mapTexture.height - 1 - y): 
               (Color32) mapTexture.GetPixel(x, y);
               // Debug.Log(pxColor);

                bool isLand = false; //地形
                SetTownType setTownType = SetTownType.None; //聚落
                TileObjectType tileObject = TileObjectType.None; //物件

                if (IsColorClose(pxColor,BLACK,colorTolerance)) { isLand = true; } //生成土地-黑
                else if (IsColorClose(pxColor, WHITE, colorTolerance)) { isLand = false; }//生成水-白
                else if (IsColorClose(pxColor, GRAY, colorTolerance)) { continue; } //不生成-灰
                else if (IsColorClose(pxColor, ORANGE, colorTolerance)) //生成聚落中心-橘
                { isLand = true; townCenters.Add(new Vector2Int(x, y)); }
                else if (IsColorClose(pxColor,YELLOW, colorTolerance)) //生成聚落開發區域-黃
                { isLand = true;townAreas.Add(new Vector2Int(x, y)); }
                else if (IsColorClose(pxColor,RED, colorTolerance)) //生成果叢-紅
                { isLand = true;tileObject = TileObjectType.FruitBush;fruitPlaces.Add(new Vector2Int(x, y)); }
                else if(IsColorClose(pxColor, BROWN, colorTolerance))  //生成草叢-棕
                {  isLand = true; tileObject = TileObjectType.InsectGrass;insectPlaces.Add(new Vector2Int(x, y)); }
                else if(IsColorClose(pxColor, BLUE, colorTolerance)) {  isLand = true;} //起點
                else if (IsColorClose(pxColor, GREEN, colorTolerance)) {  isLand = true;} //終點
                else
                {
                    Debug.Log($"座標: ({x}, {y})的TileData 為 null！");                  
                }

                tileData[x, y] = new TileData { isLand = isLand, setTownType = setTownType,tileObjectType = tileObject };

               
            }
        }

        allTownPlace = townCenters.Count;
        Debug.Log($"找到{allTownPlace}個聚落生成點,{ townAreas.Count}格開發區域");

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
        Debug.Log($"共有{townCenters.Count}個聚落");

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

        Debug.Log($"已分配{townCenters.Count}個聚落");
       
    }

    //判斷格子是否相鄰
    bool IsNeighbor(Vector2Int a,Vector2Int b) 
    {
        Vector2Int[] directions = GetHexDirections();
        foreach (var dir in directions)
        {
            if(a + dir == b)return true;
        }
        return false;
    }

    //六邊形格子相鄰方向
    Vector2Int[] GetHexDirections()
    {
        return new Vector2Int[]
        {
        new Vector2Int(1, 0), new Vector2Int(-1, 0),
        new Vector2Int(0, 1), new Vector2Int(0, -1),
        new Vector2Int(1, -1), new Vector2Int(-1, 1)
        };
    }

    //隨機聚落類型
    SetTownType GetRandomTownType()
    {
        float rand = Random.value;
        if (rand < cityRate) return SetTownType.City;
        if (rand < cityRate + villageRate) return SetTownType.Village;
        return SetTownType.Industry;
    }
}
