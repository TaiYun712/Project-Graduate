using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexUtils 
{
    public static Vector3 AxialToWorld(Vector2Int axial, float hexSize)
    {
        float x = hexSize * 1.5f * axial.x;
        float z = hexSize * Mathf.Sqrt(3) * (axial.y + 0.5f * (axial.x & 1));
        return new Vector3(x, 0, z);
    }
}
