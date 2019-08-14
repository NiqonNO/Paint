using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class BrushSettings
{
    public static BrushType brushType = BrushType.Pencil;
    public static bool smooth = true;
    public static float speed = 0.1f;
    public static int weight = 20;

    public static float fadeLength = 100f;
    public static float power = 5f;

    public static float frequency = 1f;
    public static int size = 10;
    public static float falloff = 0.5f;
    public static Color color = Color.black;
}