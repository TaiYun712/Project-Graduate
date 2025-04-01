using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject landTilePf;
    public GameObject waterTilePf;

    [Header("Towns Prefabs")]
    public GameObject[] cityTilePf;
    public GameObject[] villageTilePf;
    public GameObject[] industryTilePf;

    [Header("FoodPoint Prefabs")]
    public GameObject fruitBushPf;
    public GameObject insectGrassPf;

    Stack<GameObject> landPool = new Stack<GameObject>();
    Stack<GameObject> waterPool = new Stack<GameObject>();
    Stack<GameObject> cityPool = new Stack<GameObject>();
    Stack<GameObject> villagePool = new Stack<GameObject>();
    Stack<GameObject> industryPool = new Stack<GameObject>();

    Stack<GameObject> fruitPool = new Stack<GameObject>();
    Stack<GameObject> grassPool = new Stack<GameObject>();

    //取得隨機造型
    GameObject GetRandomPf(GameObject[] pfs)
    {
        if (pfs.Length == 0 || pfs == null) {Debug.Log("預製體數組為空"); return null; } 
        return pfs[Random.Range(0, pfs.Length)];
    }

    //取得tile
    public GameObject GetTile(TileData tileData)
    {

        if (tileData == null)
        {
            Debug.LogError("傳入的TileData為null！");
            return null;
        }

        //選擇用哪個pool&tile
        Stack<GameObject> pool;
        GameObject pf;

        if(tileData.setTownType == SetTownType.City)
        {
            pool = cityPool;
            pf = GetRandomPf(cityTilePf);
        }else if(tileData.setTownType == SetTownType.Village)
        {
            pool = villagePool;
            pf = GetRandomPf(villageTilePf);
        }
        else if(tileData.setTownType == SetTownType.Industry)
        {
            pool = industryPool;
            pf = GetRandomPf(industryTilePf);
        }
        else if (tileData.isLand)
        {
            pool = landPool;
            pf = landTilePf;
        }
        else
        {
            pool = waterPool;
            pf = waterTilePf;
        }

        return GetFromPool(pool, pf);

       
    }


    public GameObject GetTileObject(TileObjectType objectType)
    {
        Stack<GameObject> pool;
        GameObject pf;

        if(objectType == TileObjectType.FruitBush)
        {
            pool = fruitPool;
            pf = fruitBushPf;
        }
        else if(objectType == TileObjectType.InsectGrass)
        {
            pool = grassPool;
            pf = insectGrassPf;
        }
        else
        {
            return null;
        }

        return GetFromPool(pool, pf);
    }


    //取出tile
    GameObject GetFromPool(Stack<GameObject> pool,GameObject prefab)
    {

        if (prefab == null)
        {
            Debug.LogError("預製體為null，無法從對象池獲取對象！");
            return null;
        }

        GameObject obj =(pool.Count > 0)? pool.Pop() : Instantiate(prefab);
        obj.SetActive(true);
        return obj;
    }

    //回收tile
    public void ReturnTile(GameObject tile,TileData tileData)
    {
        tile.SetActive(false);

        if (tileData.setTownType == SetTownType.City) { cityPool.Push(tile); }
        else if (tileData.setTownType == SetTownType.Village) { villagePool.Push(tile); }
        else if (tileData.setTownType == SetTownType.Industry) { industryPool.Push(tile); }   
        else if (tileData.isLand) { landPool.Push(tile); }
        else { waterPool.Push(tile); }
           
    }

    //回收果叢&草叢
    public void ReturnObject(GameObject obj, TileObjectType objectType)
    {
        Stack<GameObject> pool;

        if (objectType == TileObjectType.FruitBush)
        {
            pool = fruitPool;
        }
        else if (objectType == TileObjectType.InsectGrass)
        {
            pool = grassPool;
        }
        else
        {
            return;
        }

        obj.SetActive(false);
        pool.Push(obj);
    }

}
