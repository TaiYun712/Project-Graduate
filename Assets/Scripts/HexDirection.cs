using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDirection 
{
    // ���Ӥ�V�s���G0~5�]�����ɰw�ΰf�ɰw����^
    public static readonly Vector2Int[] EvenQ_Directions = new Vector2Int[]
    {
        new Vector2Int(+1,  0),  // �k
        new Vector2Int( 0, +1),  // �k�U
        new Vector2Int(-1, +1),  // ���U
        new Vector2Int(-1,  0),  // ��
        new Vector2Int(-1, -1),  // ���W
        new Vector2Int( 0, -1)   // �k�W
    };

    public static readonly Vector2Int[] OddQ_Directions = new Vector2Int[]
    {
        new Vector2Int(+1,  0),  // �k
        new Vector2Int(+1, +1),  // �k�U
        new Vector2Int( 0, +1),  // ���U
        new Vector2Int(-1,  0),  // ��
        new Vector2Int( 0, -1),  // ���W
        new Vector2Int(+1, -1)   // �k�W
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
