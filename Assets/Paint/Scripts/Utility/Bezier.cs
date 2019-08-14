using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{
    public static Vector3 GetQuadraticBezierMiddle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return .25f * p1 + .5f * p2 + .25f * p3;
    }
}
