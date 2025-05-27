using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_Test : MonoBehaviour
{
    [Header("取得資訊")]
    public Camera mainCam;
    public GameObject selectHintPf;
    public float hintHeight = 0.3f;

    [Header("拼接Tile")]
    public LayerMask mapTileLayer;
    public float snapDistance = 0.5f;
    public GameObject placeableHintPf;

    GameObject heldTile;
    Vector3 offset = Vector3.zero;
    bool isHolding = false;

    void Start()
    {
        selectHintPf.SetActive(false);
        placeableHintPf.SetActive(false);
    }

    void Update()
    {
        RightClickToGetTileInfo();
        LeftClickToDragAndSnap();
    }

    //按滑鼠右鍵取得tile屬性資訊
    void RightClickToGetTileInfo()
    {
        if (Input.GetMouseButtonDown(1))
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

    //按滑鼠左鍵拖曳拼接tile
    void LeftClickToDragAndSnap()
    {
        //拿起
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay (Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hit, 100f))
            {
                if (hit.collider.CompareTag("PlayerTile"))
                {
                    heldTile = hit.collider.gameObject;
                    offset = heldTile.transform.position - hit.point;
                    isHolding = true;

                    if (!placeableHintPf.activeSelf) 
                    { 
                      placeableHintPf.SetActive(true); 
                    }
                }
            }
        }

        //拖曳
        if(isHolding && heldTile != null)
        {
            Plane plane = new Plane(Vector3.up,Vector3.zero); //zx平面
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray,out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                heldTile.transform.position = new Vector3(point.x,0f,point.z) + offset;

                //找最近地圖tile
                Collider[] hits = Physics.OverlapSphere(heldTile.transform.position,snapDistance,mapTileLayer);
                if(hits.Length > 0)
                {
                    Transform nearest = hits[0].transform;
                    placeableHintPf.transform.position = nearest.position;
                    placeableHintPf.SetActive(true);
                }
                else
                {
                    placeableHintPf.SetActive(false);
                }
            }
        }

        //放開
        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            isHolding = false;

            if(placeableHintPf.activeSelf && heldTile != null)
            {
                heldTile.transform.position = placeableHintPf.transform.position;
                heldTile.layer = LayerMask.NameToLayer("MapTile");
                heldTile.tag = "GameMapTile";
            }

            heldTile = null;
            placeableHintPf.SetActive(false);

        }

    }

}
