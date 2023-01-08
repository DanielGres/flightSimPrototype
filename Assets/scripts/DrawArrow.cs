
using UnityEngine;
using System.Collections;

public static class DrawArrow
{
    public static void ForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, direction * arrowHeadLength, color);

    }
}
