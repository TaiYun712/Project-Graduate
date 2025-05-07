using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_Test : MonoBehaviour
{
    public Camera mainCam;
    public GameObject selectHintPf;
    public float hintHeight = 0.3f;

    void Start()
    {
        
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 50f, Color.blue);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Raycast Hit: " + hit.collider.name);

                TileBehaviour tile = hit.collider.GetComponent<TileBehaviour>();

                if (tile != null)
                {
                    selectHintPf.SetActive(true);
                    selectHintPf.transform.position = tile.transform.position + Vector3.up * hintHeight;
                    Debug.Log($"座標位置{tile.gridPos}，地形為{(tile.IsLand ? "陸地" : "水")}，聚落為{tile.tileData.setTownType}");

                }
                else
                {
                    selectHintPf.SetActive(false);
                }
            }
        }
       
       
    }
}
