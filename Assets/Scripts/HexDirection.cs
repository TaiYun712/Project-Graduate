using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDirection 
{
    // 六個方向編號：0~5（按順時針或逆時針都行）
    public static readonly Vector2Int[] EvenQ_Directions = new Vector2Int[]
    {
        new Vector2Int(+1,  0),  // 右
        new Vector2Int( 0, +1),  // 右下
        new Vector2Int(-1, +1),  // 左下
        new Vector2Int(-1,  0),  // 左
        new Vector2Int(-1, -1),  // 左上
        new Vector2Int( 0, -1)   // 右上
    };

    public static readonly Vector2Int[] OddQ_Directions = new Vector2Int[]
    {
        new Vector2Int(+1,  0),  // 右
        new Vector2Int(+1, +1),  // 右下
        new Vector2Int( 0, +1),  // 左下
        new Vector2Int(-1,  0),  // 左
        new Vector2Int( 0, -1),  // 左上
        new Vector2Int(+1, -1)   // 右上
    };

    public static Vector2Int GetNeighbor(Vector2Int current, int direction)
    {
        if(current.x%2 == 0)
        {
            return current + EvenQ_Directions[direction];
        }
        else
        {
            return current + OddQ_Directions[direction];
        }

    }

    public static List<Vector2Int> GetAllNeighbor(Vector2Int current)
    {
        var list = new List<Vector2Int>();
        for (int i = 0; i < 6; i++)
        {
            list.Add(GetNeighbor(current, i));
        }
        return list;
    }


}
