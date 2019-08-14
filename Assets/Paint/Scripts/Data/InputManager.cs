using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    static bool cursorOnCanvas;
    public static bool CursorOnCanvas { get { return cursorOnCanvas; } }
    public static bool LeftClick { get { return Input.GetMouseButton(0); } }
    public static bool LeftClickDown { get { return Input.GetMouseButtonDown(0); } }
    public static Vector3 MousePosition
    {
        get
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            return mousePos;
        }
    }
    public static void SetCursorOnCanvasValue(bool value)
    {
        cursorOnCanvas = value;
    }
}
