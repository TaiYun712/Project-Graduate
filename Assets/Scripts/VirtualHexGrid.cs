using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    //搜尋地圖邊緣的空格
    public List<Vector2Int> GetAttachablePositions()
    {
        HashSet<Vector2Int> candidates = new HashSet<Vector2Int>();

        foreach (var pair in grid)
        {
            Vector2Int pos = pair.Key;

            // 檢查六個方向
            for (int dir = 0; dir < 6; dir++)
            {
                Vector2Int neighborPos = HexDirection.GetNeighbor(pos, dir);

                // 如果這個鄰居還沒被佔據
                if (!HasTile(neighborPos))
                {
                    candidates.Add(neighborPos);
                }
            }
        }

        return candidates.ToList();
    }

    //將虛擬六邊形座標轉換為世界座標（中心點）
    public Vector2 GridToWorld(Vector2Int gridPos, float tileSize)
    {
        float width = tileSize * 2f;
        float height = Mathf.Sqrt(3f) * tileSize;

        float x = gridPos.x * (width * 0.75f); // 偏移列
        float z = gridPos.y * height; // 奇偶列偏移

        if (gridPos.x % 2 != 0)
        {
            z += height * 0.5f;
        }

        return new Vector2(x, z);
    }
}
