using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintHandle : MonoBehaviour
{
    [SerializeField] ComputeShader compute;

    [HideInInspector] public float boundaryLeft, boundaryRight, boundaryUp, boundaryDown;

    int width = 1200;
    int height = 900;

    private void Awake()
    {
        boundaryLeft = transform.position.x - 0.5f * transform.localScale.x;
        boundaryRight = transform.position.x + 0.5f * transform.localScale.x;
        boundaryDown = transform.position.y - 0.5f * transform.localScale.y;
        boundaryUp = transform.position.y + 0.5f * transform.localScale.y;

        InitializeTexture();
    }
    private void OnEnable()
    {
        UIController.OnShaderDataNeedUpdate += UpdateData;
    }
    private void OnDisable()
    {
        UIController.OnShaderDataNeedUpdate -= UpdateData;
    }
    private void OnMouseEnter()
    {
        InputManager.SetCursorOnCanvasValue(true);
    }
    private void OnMouseExit()
    {
        InputManager.SetCursorOnCanvasValue(false);
    }

    public void Paint(Vector3 start, Vector3 end, ref int timer)
    {
        start = ToPixel(start);
        end = ToPixel(end);

        do {
            compute.SetFloat("timer", Mathf.Pow(Mathf.Clamp01(1 - timer / BrushSettings.fadeLength), BrushSettings.power));
            timer++;
            start = Vector3.MoveTowards(start, end, BrushSettings.frequency);
            compute.SetVector("coordinate", new Vector2(start.x, start.y));//, end.x, end.y));
            compute.Dispatch((int)BrushSettings.brushType, Mathf.CeilToInt(width / 32f), Mathf.CeilToInt(height / 32f), 1);
        } while (start != end);
    }

    private void InitializeTexture()
    {
        RenderTexture ttr = RenderTextureCreator.CreateRenderTexture(new Color(1, 1, 1, 1), width, height, 32);
        compute.SetTexture(0, "Result", ttr);
        compute.SetTexture(1, "Result", ttr);
        compute.SetTexture(2, "Result", ttr);
        transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", ttr);
    }
    private void UpdateData()
    {
        compute.SetFloat("brushSize", BrushSettings.size);
        compute.SetFloat("falloff", BrushSettings.size * BrushSettings.falloff);
        compute.SetVector("color", BrushSettings.color);
    }
    private Vector3 ToPixel(Vector3 point)
    {
        point = transform.InverseTransformPoint(point);

        point.x = Mathf.Clamp(point.x * width + (width / 2f), 0, width);
        point.y = Mathf.Clamp(point.y * height + (height / 2f), 0, height);

        return point;
    }
    /*private Vector3 SmoothMoveTowards(Vector3 direction, Vector3 start, Vector3 end, float maxDistance)
    {
        Vector3 tang = start + (direction * Vector3.Distance(start, end) / 10f);

        return Bezier.GetQuadraticBezierMiddle(start, tang, end);
    }*/
}
