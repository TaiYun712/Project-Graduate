using System.Collections;
using System.Collections.Generic;
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
}
