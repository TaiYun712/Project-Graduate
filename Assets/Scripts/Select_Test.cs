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
    public float planeHeight;
    public Transform mapPlane;
    public float hexTileSize;
    public float maxCheckDistance;

    GameObject heldTile;
    Vector3 offset = Vector3.zero;
    bool isHolding = false;

    [Header("參考 MapMaker")]
    public MapMaker mapMaker;

    List<Vector2Int> attachablePositions;

    void Start()
    {
        planeHeight = mapPlane.position.y;

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
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.CompareTag("PlayerTile"))
                {
                    heldTile = hit.collider.gameObject;

                    isHolding = true;

                    if (!placeableHintPf.activeSelf)
                    {
                        placeableHintPf.SetActive(true);
                    }

                    //取得合法拼接位置清單
                    attachablePositions = mapMaker.HexGrid.GetAttachablePositions();
                }
            }
        }

        //拖曳
        if (isHolding && heldTile != null)
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0, planeHeight, 0)); //zx平面
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                heldTile.transform.position = new Vector3(point.x, planeHeight, point.z);

                //找最近地圖tile
                /*
                float minSnapDist = float.MaxValue;
                Vector3 bestSnapPos = Vector3.zero;
                bool foundSnap = false;

                foreach(Vector2Int gridPos in attachablePositions)
                {
                    Vector2 worldXZ = mapMaker.HexGrid.GridToWorld(gridPos,hexTileSize);
                    Vector3 candidatePos = new Vector3(worldXZ.x, planeHeight, worldXZ.y);

                    float dist = Vector3.Distance(heldTile.transform.position, candidatePos);
                    if(dist < minSnapDist)
                    {
                        minSnapDist = dist;
                        bestSnapPos = candidatePos;
                        foundSnap = true;
                    }
                }

                if(foundSnap && minSnapDist < maxCheckDistance)
                {
                    placeableHintPf.transform.position = bestSnapPos;
                    placeableHintPf.SetActive(true);
                }
                else
                {
                    placeableHintPf.SetActive(false);

                }
                */

                Vector3[] hexDirs = new Vector3[]
                {
                new Vector3(1.5f * hexTileSize, 0, 0),                                       // 右
                new Vector3(0.75f * hexTileSize, 0, Mathf.Sqrt(3)/2 * hexTileSize),         // 右上
                new Vector3(-0.75f * hexTileSize, 0, Mathf.Sqrt(3)/2 * hexTileSize),        // 左上
                new Vector3(-1.5f * hexTileSize, 0, 0),                                      // 左
                new Vector3(-0.75f * hexTileSize, 0, -Mathf.Sqrt(3)/2 * hexTileSize),       // 左下
                new Vector3(0.75f * hexTileSize, 0, -Mathf.Sqrt(3)/2 * hexTileSize)         // 右下
                };

                Collider[] hits = Physics.OverlapSphere(heldTile.transform.position, snapDistance, mapTileLayer);
                float minDist = float.MaxValue;
                Collider nearestCollider = null;
                
                foreach (var hit in hits)
                {
                    float dist = Vector3.Distance(heldTile.transform.position, hit.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestCollider = hit;
                    }
                }

                     

                if (nearestCollider != null && minDist < maxCheckDistance)
                {
                    Transform nearest = nearestCollider.transform;

                    Vector3 bestSnapPos = Vector3.zero;
                    float minSnapDist = float.MaxValue;

                    foreach (Vector3 dir in hexDirs)
                    {
                        Vector3 candidatePos = nearest.position + dir;
                        float dist = Vector3.Distance(heldTile.transform.position, candidatePos);

                        if (dist < minSnapDist)
                        {
                            minSnapDist = dist;
                            bestSnapPos = candidatePos;
                        }

                    }

                    placeableHintPf.transform.position = bestSnapPos;
                    placeableHintPf.SetActive(true);

                }
                else
                {
                    placeableHintPf.SetActive(false);

                }
                
            }

            //放開
            if (Input.GetMouseButtonUp(0) && isHolding)
            {
                isHolding = false;

                if (placeableHintPf.activeSelf && heldTile != null)
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
}
    
      

    
