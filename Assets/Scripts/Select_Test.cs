using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_Test : MonoBehaviour
{
    [Header("���o��T")]
    public Camera mainCam;
    public GameObject selectHintPf;
    public float hintHeight = 0.3f;

    [Header("����Tile")]
    public LayerMask mapTileLayer;
    public float snapDistance = 0.5f;
    public GameObject placeableHintPf;
    public float planeHeight;
    public Transform mapPlane;
    public float hexTileSize;

    GameObject heldTile;
    Vector3 offset = Vector3.zero;
    bool isHolding = false;

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

    //���ƹ��k����otile�ݩʸ�T
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
                    Debug.Log($"�y�Ц�m{tile.gridPos}�A�a�ά�{(tile.IsLand ? "���a" : "��")}�A�E����{tile.tileData.setTownType}");

                }
                else
                {
                    selectHintPf.SetActive(false);
                }
            }
        }
    }

    //���ƹ�����즲����tile
    void LeftClickToDragAndSnap()
    {
        //���_
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

        //�즲
        if(isHolding && heldTile != null)
        {
            Plane plane = new Plane(Vector3.up,new Vector3 (0,planeHeight,0)); //zx����
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray,out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                heldTile.transform.position = new Vector3(point.x,planeHeight,point.z) + offset;

                //��̪�a��tile
                Collider[] hits = Physics.OverlapSphere(heldTile.transform.position,snapDistance,mapTileLayer);

                Collider nearestCollider = null;
                float minDist = float.MaxValue;

                foreach(var hit in hits)
                {
                    float dist = Vector3.Distance(heldTile.transform.position, hit.transform.position);
                    if(dist < minDist)
                    {
                        minDist = dist;
                        nearestCollider = hit;
                    }
                }

                if(nearestCollider !=null)
                {
                    Transform nearest = nearestCollider.transform;
                    Vector3 direction = (heldTile.transform.position - nearest.position).normalized;
                    Vector3 snapPos = nearest.position + direction * hexTileSize *2.0f;

                    placeableHintPf.transform.position = snapPos;
                    placeableHintPf.SetActive(true);
                }
                else
                {
                    placeableHintPf.SetActive(false);
                }
            }
        }

        //��}
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
