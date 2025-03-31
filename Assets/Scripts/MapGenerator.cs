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

    [Header("TownSetting")]
    [SerializeField] int allTownPlace;  
    public float cityRate;
    public float villageRate;


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
                mapTexture.GetPixel(x, mapTexture.height - 1 - y): 
                mapTexture.GetPixel(x, y);
               
                bool isLand = false; //地形
                SetTownType setTownType = SetTownType.None; //聚落
                TileObjectType tileObject = TileObjectType.None; //物件

                if (IsColorClose(pxColor,new Color32(0,0,0,255))) { isLand = true; } //生成土地-黑
                else if (IsColorClose(pxColor, new Color32(255, 255, 255, 255))) { isLand = false; }//生成水-白
                else if (IsColorClose(pxColor, new Color32(128,128, 128, 255))) { continue; } //不生成-灰
                else if (IsColorClose(pxColor, new Color32(255, 128, 0, 255))) //生成聚落中心-橘
                { isLand = true; townCenters.Add(new Vector2Int(x, y)); }
                else if (IsColorClose(pxColor, new Color32(255, 255, 0, 255))) //生成聚落開發區域-黃
                { isLand = true;townAreas.Add(new Vector2Int(x, y)); }
                else if (IsColorClose(pxColor, new Color32(255, 0, 0, 255))) //生成果叢-紅
                { isLand = true;tileObject = TileObjectType.FruitBush;fruitPlaces.Add(new Vector2Int(x, y)); }
                else if(IsColorClose(pxColor, new Color32(165, 42, 42, 255)))  //生成草叢-棕
                {  isLand = true; tileObject = TileObjectType.InsectGrass;insectPlaces.Add(new Vector2Int(x, y)); }
                else if(IsColorClose(pxColor, new Color32(0, 0, 255, 255))) {  isLand = true;} //起點
                else if (IsColorClose(pxColor, new Color32(0, 255, 0, 255))) {  isLand = true;} //終點


                tileData[x, y] = new TileData { isLand = isLand, setTownType = setTownType,tileObjectType = tileObject };

               
            }
        }

        allTownPlace = townCenters.Count;
        Debug.Log($"找到{allTownPlace}個聚落生成點,{ townAreas.Count}格開發區域");

        AssignTowns(townCenters,townAreas);
    }

    bool IsColorClose(Color32 a,Color32 b,int tolerance = 10)
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
