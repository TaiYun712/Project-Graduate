using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public GameObject grassTile;

    Queue<GameObject> grassTilePool = new Queue<GameObject>();

    public int poolSize;
    public Transform mapParent;

    void Start()
    {
        PrewarmPool();
    }

    void PrewarmPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tile = Instantiate(grassTile,mapParent);
            tile.SetActive(false); //������
            grassTilePool.Enqueue(tile); //��i����
        }
    }

    public GameObject GetTile(Vector3 pos)
    {
        GameObject tile;
        if(grassTilePool.Count > 0)
        {
            tile = grassTilePool.Dequeue();
        }
        else //�p�G���̪ŤF�N�A�ͦ�
        {
            tile = Instantiate(grassTile,mapParent);
        }

        tile.transform.position = pos;
        tile.SetActive(true);
        return tile;
    }

    public void ReturnTile(GameObject tile)
    {
        tile.SetActive(false);
        grassTilePool.Enqueue(tile);
    }

   
}
