using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualHexGrid
{
   Dictionary<Vector2Int,VirtualHexTile> grid = new Dictionary<Vector2Int,VirtualHexTile>();

    // 新增或覆蓋格子資料
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

    // 取得指定格子（若不存在回傳 null）
    public VirtualHexTile GetTile(Vector2Int pos)
    {
        return grid.ContainsKey(pos) ? grid[pos] : null;
    }

    // 該位置是否有資料
    public bool HasTile(Vector2Int pos)
    {
        return grid.ContainsKey(pos);
    }

    // 取得所有格子的資料
    public IEnumerable<KeyValuePair<Vector2Int,VirtualHexTile>> GetAllTiles()
    {
        return grid;
    }
}
