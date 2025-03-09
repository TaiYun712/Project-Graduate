using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapGenerate : MonoBehaviour
{
    [Range(0f, 100f)]
    public int mapWidth;
    [Range(0f, 100f)]
    public int mapHeight;

    public float tileSize;
 
    Vector2 hexCoords;

    public GameObject grassLandTile;

    public TileFactory tileFactory;


   
    void Start()
    {
       MakeMapGrid();
    }

    Vector2 GetHexCoords(int x,int z) // 調整六邊形的連接位置
    {
        float xPos = x * tileSize * Mathf.Cos(Mathf.Deg2Rad*30);
        float zPos = z * tileSize  + ((x % 2 == 1) ? tileSize * 0.5f : 0);

        return new Vector2(xPos, zPos);
    }

    public void MakeMapGrid()
    {
       
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                           
                hexCoords = GetHexCoords(x,z);

                Vector3 pos = new Vector3(hexCoords.x,0,hexCoords.y);
                tileFactory.GetTile(pos);
                
            }
        }
    } 
}
