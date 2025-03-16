using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject landTilePf;
    public GameObject waterTilePf;
    public GameObject cityTilePf;
    public GameObject villageTilePf;
    public GameObject industryTilePf;

    Stack<GameObject> landPool = new Stack<GameObject>();
    Stack<GameObject> waterPool = new Stack<GameObject>();
    Stack<GameObject> cityPool = new Stack<GameObject>();
    Stack<GameObject> villagePool = new Stack<GameObject>();
    Stack<GameObject> industryPool = new Stack<GameObject>();


    //取得tile
    public GameObject GetTile(TileData tileData)
    {
        //選擇用哪個pool&tile
        Stack<GameObject> pool;
        GameObject pf;

        if(tileData.setTownType == SetTownType.City)
        {
            pool = cityPool;
            pf = cityTilePf;
        }else if(tileData.setTownType == SetTownType.Village)
        {
            pool = villagePool;
            pf = villageTilePf;
        }else if(tileData.setTownType == SetTownType.Industry)
        {
            pool = industryPool;
            pf = industryTilePf;
        }else if (tileData.isLand)
        {
            pool = landPool;
            pf = landTilePf;
        }
        else
        {
            pool = waterPool;
            pf = waterTilePf;
        }

        GameObject tile;

        if (pool.Count > 0)
        {
            tile = pool.Pop();
            tile.SetActive(true);
        }
        else
        {
            tile = Instantiate(pf);
        }

        return tile;
    }

    //回收tile
    public void ReturnTile(GameObject tile,TileData tileData)
    {
        tile.SetActive(false);

        if (tileData.setTownType == SetTownType.City)
            cityPool.Push(tile);
        else if(tileData.setTownType == SetTownType.Village)
            villagePool.Push(tile);
        else if(tileData.setTownType == SetTownType.Industry)
            industryPool.Push(tile);
        else if(tileData.isLand)
            landPool.Push(tile);
        else
            waterPool.Push(tile);
    }
  
}
