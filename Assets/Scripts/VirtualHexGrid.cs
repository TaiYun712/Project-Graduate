using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VirtualHexGrid
{
   Dictionary<Vector2Int,VirtualHexTile> grid = new Dictionary<Vector2Int,VirtualHexTile>();

    // �s�W���л\��l���
    public void SetTile(Vector2Int pos,TileData data)
    {
        if (grid.ContainsKey(pos))
        {
            grid[pos].tileData = data;
        }
        else
        {
            grid[pos] = new VirtualHexTile(pos,data);
        }
    }

    // ���o���w��l�]�Y���s�b�^�� null�^
    public VirtualHexTile GetTile(Vector2Int pos)
    {
        return grid.ContainsKey(pos) ? grid[pos] : null;
    }

    // �Ӧ�m�O�_�����
    public bool HasTile(Vector2Int pos)
    {
        return grid.ContainsKey(pos);
    }

    // ���o�Ҧ���l�����
    public IEnumerable<KeyValuePair<Vector2Int,VirtualHexTile>> GetAllTiles()
    {
        return grid;
    }

    //�j�M�a����t���Ů�
    public List<Vector2Int> GetAttachablePositions()
    {
        HashSet<Vector2Int> candidates = new HashSet<Vector2Int>();

        foreach (var pair in grid)
        {
            Vector2Int pos = pair.Key;

            // �ˬd���Ӥ�V
            for (int dir = 0; dir < 6; dir++)
            {
                Vector2Int neighborPos = HexDirection.GetNeighbor(pos, dir);

                // �p�G�o�ӾF�~�٨S�Q����
                if (!HasTile(neighborPos))
                {
                    candidates.Add(neighborPos);
                }
            }
        }

        return candidates.ToList();
    }

    //�N��������ήy���ഫ���@�ɮy�С]�����I�^
    public Vector2 GridToWorld(Vector2Int gridPos, float tileSize)
    {
        float width = tileSize * 2f;
        float height = Mathf.Sqrt(3f) * tileSize;

        float x = gridPos.x * (width * 0.75f); // �����C
        float z = gridPos.y * height; // �_���C����

        if (gridPos.x % 2 != 0)
        {
            z += height * 0.5f;
        }

        return new Vector2(x, z);
    }
}
