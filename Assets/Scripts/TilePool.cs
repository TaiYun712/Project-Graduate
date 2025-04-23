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

   

    Stack<GameObject> landPool = new Stack<GameObject>();
    Stack<GameObject> waterPool = new Stack<GameObject>();
    Stack<GameObject> cityPool = new Stack<GameObject>();
    Stack<GameObject> villagePool = new Stack<GameObject>();
    Stack<GameObject> industryPool = new Stack<GameObject>();

    //���o�H���y��
    GameObject GetRandomPf(GameObject[] pfs)
    {
        if (pfs.Length == 0 || pfs == null) {Debug.Log("�w�s��Ʋլ���"); return null; } 
        return pfs[Random.Range(0, pfs.Length)];
    }

    //���otile
    public GameObject GetTile(TileData tileData)
    {
        
        if (tileData == null)
        {
            Debug.LogError("�ǤJ��TileData��null�I");
            return null;
        }

        //��ܥέ���pool&tile
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

        GameObject tileGO =  GetFromPool(pool, pf);

        var tileBehavior = pf.GetComponent<TileBehavior>();
        if(tileBehavior != null)
        {
            tileBehavior.tileData = tileData;
        }

        return tileGO;
    }


   


    //���Xtile
    GameObject GetFromPool(Stack<GameObject> pool,GameObject prefab)
    {

        if (prefab == null)
        {
            Debug.LogError("�w�s�鬰null�A�L�k�q��H�������H�I");
            return null;
        }

        GameObject obj =(pool.Count > 0)? pool.Pop() : Instantiate(prefab);
        obj.SetActive(true);
        return obj;
    }

    //�^��tile
    public void ReturnTile(GameObject tile,TileData tileData)
    {
        tile.SetActive(false);

        if (tileData.setTownType == SetTownType.City) { cityPool.Push(tile); }
        else if (tileData.setTownType == SetTownType.Village) { villagePool.Push(tile); }
        else if (tileData.setTownType == SetTownType.Industry) { industryPool.Push(tile); }   
        else if (tileData.isLand) { landPool.Push(tile); }
        else { waterPool.Push(tile); }
           
    }

   

}
