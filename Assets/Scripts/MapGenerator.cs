using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSettings")]
    public Texture2D mapTexture; //黑白地圖
    public bool flipY = true;

    public int mapWidth;
    public int mapHeight;

    public TileData[,] tileData; //儲存產生好的地圖資料

    [Header("TownSetting")]
    [SerializeField] int allTownPlace;  //
    public int generateTownsCount; //自訂聚落生成數量
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
        List<Vector2Int> townPlaces = new List<Vector2Int>(); //存放可生成聚落的位置

        //分析每個像素
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Color pxGray = flipY ?
                mapTexture.GetPixel(x, mapTexture.height - 1 - y): 
                mapTexture.GetPixel(x, y);

                float graySC = pxGray.grayscale;
                bool isLand = graySC < 0.5f;
                SetTownType setTownType = SetTownType.None;

                //紀錄灰色區域為聚落可生成點
                if(graySC >0.3f && graySC < 0.7f)
                {
                    townPlaces.Add(new Vector2Int(x, y));
                }

                tileData[x, y] = new TileData { isLand =  isLand ,setTownType = setTownType};
            }
        }

        allTownPlace = townPlaces.Count;
        Debug.Log($"找到{allTownPlace}個聚落生成點");

        AssignTowns(townPlaces);
    }

    void AssignTowns(List<Vector2Int> theTownPlaces)
    {
        if (theTownPlaces.Count == 0) return;

        int count = Mathf.Min(generateTownsCount, theTownPlaces.Count);//限制數量
        //打亂後選出要生成聚落的點
        List<Vector2Int> selectedPoints = theTownPlaces.OrderBy(x => Random.value).Take(count).ToList();

        //分配聚落類型
        for (int i = 0; i < selectedPoints.Count; i++)
        {
            SetTownType type;
            float rand = Random.value;

            if (rand < cityRate) type = SetTownType.City;
            else if (rand < villageRate) type = SetTownType.Village;
            else type = SetTownType.Industry;

            Vector2Int pos = selectedPoints[i];
            tileData[pos.x, pos.y].setTownType = type;
        }

        Debug.Log($"已分配{count}個聚落");
    }
}
