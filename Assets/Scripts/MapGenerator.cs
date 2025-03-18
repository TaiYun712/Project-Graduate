using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("MapSettings")]
    public Texture2D mapTexture; //�¥զa��
    public bool flipY = true;

    public int mapWidth;
    public int mapHeight;

    public TileData[,] tileData; //�x�s���ͦn���a�ϸ��

    [Header("TownSetting")]
    [SerializeField] int allTownPlace;  //
    public int generateTownsCount; //�ۭq�E���ͦ��ƶq
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
            Debug.LogError("�S��a��");
            return;
        }

        //�Y�����w���e�N�ιϤ��ؤo
        if(mapWidth ==0) mapWidth = mapTexture.width;
        if(mapHeight ==0) mapHeight = mapTexture.height;

        tileData = new TileData[mapWidth, mapHeight];
        List<Vector2Int> townPlaces = new List<Vector2Int>(); //�s��i�ͦ��E������m

        //���R�C�ӹ���
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

                //�����Ǧ�ϰ쬰�E���i�ͦ��I
                if(graySC >0.3f && graySC < 0.7f)
                {
                    townPlaces.Add(new Vector2Int(x, y));
                }

                tileData[x, y] = new TileData { isLand =  isLand ,setTownType = setTownType};
            }
        }

        allTownPlace = townPlaces.Count;
        Debug.Log($"���{allTownPlace}�ӻE���ͦ��I");

        AssignTowns(townPlaces);
    }

    void AssignTowns(List<Vector2Int> theTownPlaces)
    {
        if (theTownPlaces.Count == 0) return;

        int count = Mathf.Min(generateTownsCount, theTownPlaces.Count);//����ƶq
        //���ë��X�n�ͦ��E�����I
        List<Vector2Int> selectedPoints = theTownPlaces.OrderBy(x => Random.value).Take(count).ToList();

        //���t�E������
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

        Debug.Log($"�w���t{count}�ӻE��");
    }
}
