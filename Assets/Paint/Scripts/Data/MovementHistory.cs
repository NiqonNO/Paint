using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementHistory
{
    List<Vector3> list;
    public void Initialize()
    {
        list = new List<Vector3>();
    }
    public void Add(Vector3 point)
    {
        list.Add(point);
        while (list.Count > BrushSettings.weight)
            list.RemoveAt(0);
    }
    public void Remove()
    {
        list.RemoveAt(0);
    }
    public void Clear()
    {
        list.Clear();
    }
    public bool GetAverge(out Vector3 result)
    {
        result = Vector3.zero;
        if (list.Count < 3)
            return false;

        SmoothPath();
        result = Smooth(1);
        return true;
    }
    private void SmoothPath()
    {
        for (int i = 1; i < list.Count - 1; i++)
            list[i] = Smooth(i);
    }
    public Vector3 Smooth(int idx)
    {
        return Bezier.GetQuadraticBezierMiddle(list[idx - 1], list[idx], list[idx + 1]);
    }

}
