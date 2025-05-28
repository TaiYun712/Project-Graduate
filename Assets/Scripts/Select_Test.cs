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
                }
            }
        }

        //�즲
        if (isHolding && heldTile != null)
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0, planeHeight, 0)); //zx����
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 point = ray.GetPoint(enter);
                heldTile.transform.position = new Vector3(point.x, planeHeight, point.z);

                //��̪�a��tile
                Collider[] hits = Physics.OverlapSphere(heldTile.transform.position, snapDistance, mapTileLayer);

                Collider nearestCollider = null;
                float minDist = float.MaxValue;

                foreach (var hit in hits)
                {
                    float dist = Vector3.Distance(heldTile.transform.position, hit.transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestCollider = hit;
                    }
                }

                //����Ϊ����Ӥ�V
                Vector3[] hexDirs = new Vector3[]
                {
                new Vector3(1.5f * hexTileSize, 0, 0),                                       // �k
                new Vector3(0.75f * hexTileSize, 0, Mathf.Sqrt(3)/2 * hexTileSize),         // �k�W
                new Vector3(-0.75f * hexTileSize, 0, Mathf.Sqrt(3)/2 * hexTileSize),        // ���W
                new Vector3(-1.5f * hexTileSize, 0, 0),                                      // ��
                new Vector3(-0.75f * hexTileSize, 0, -Mathf.Sqrt(3)/2 * hexTileSize),       // ���U
                new Vector3(0.75f * hexTileSize, 0, -Mathf.Sqrt(3)/2 * hexTileSize)         // �k�U
                };

                if (nearestCollider != null)
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

            }

            //��}
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
    
      

    
