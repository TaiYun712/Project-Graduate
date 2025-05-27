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
            Plane plane = new Plane(Vector3.up,Vector3.zero); //zx����
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray,out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                heldTile.transform.position = new Vector3(point.x,0f,point.z) + offset;

                //��̪�a��tile
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
