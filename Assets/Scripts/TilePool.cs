using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePool : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public GameObject landTilePf;
    public GameObject waterTilePf;

    Stack<GameObject> landPool = new Stack<GameObject>();
    Stack<GameObject> waterPool = new Stack<GameObject>();

    //取得tile
    public GameObject GetTile(bool isLand)
    {
        GameObject tile = null;

        //選擇用哪個pool&tile
        Stack<GameObject> pool = isLand ? landPool : waterPool;
        GameObject pf = isLand? landTilePf: waterTilePf;

        if(pool.Count > 0)
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
    public void ReturnTile(GameObject tile,bool isLand)
    {
        tile.SetActive(false);

        if (isLand)
        {
            landPool.Push(tile);
        }
        else
        {
            waterPool.Push(tile);
        }
    }
  
}
