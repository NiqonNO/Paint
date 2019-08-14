using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmoothCursor : MonoBehaviour
{
    [SerializeField] PaintHandle ph;
    [SerializeField] DataManager dm;
    [SerializeField] ComputeShader compute;

    MovementHistory cursorHistory = new MovementHistory();
    Vector3 lastPosition;
    bool smoothPainting = false;
    int timer = 0;

    private void Awake()
    {
        lastPosition = InputManager.MousePosition;
        cursorHistory.Initialize();
        InitializeBrush();
    }
    private void OnEnable()
    {
        UIController.OnBrushNeedUpdate += UpdateBrush;
    }
    private void OnDisable()
    {
        UIController.OnBrushNeedUpdate -= UpdateBrush;
    }
    private void FixedUpdate()
    {
        if(InputManager.CursorOnCanvas || smoothPainting)
        {
            HandleCursor();
        }
    }

    private void HandleCursor()
    {
        if (InputManager.LeftClick)
        {
            if (BrushSettings.smooth)
            {
                smoothPainting = true;
                cursorHistory.Add(ClampedPosition(InputManager.MousePosition));
                Vector3 mousePos;
                if (cursorHistory.GetAverge(out mousePos))
                {
                    transform.position = Vector3.MoveTowards(transform.position, mousePos, Vector3.Distance(transform.position, mousePos) * BrushSettings.speed);
                    if (Vector3.Distance(transform.position, mousePos) <=1f)
                        cursorHistory.Remove();
                }
            }
            else
                transform.position = ClampedPosition(InputManager.MousePosition);

            ph.Paint(lastPosition, transform.position, ref timer);
        }
        else
        {
            ClearData();
        }
        lastPosition = transform.position;
    }
    private void InitializeBrush()
    {
        RenderTexture ttr = RenderTextureCreator.CreateRenderTexture(new Color(0, 0, 0, 0), dm.maxSize*2, dm.maxSize*2, 0);
        compute.SetInt("halfTexSize", dm.maxSize);
        compute.SetTexture(0, "Result", ttr);
        transform.GetComponent<RectTransform>().sizeDelta = Vector2.one * dm.maxSize * 2;
        transform.GetComponent<RawImage>().texture = ttr;
    }
    private void UpdateBrush()
    {
        compute.SetInt("size", BrushSettings.size);
        compute.Dispatch(0, Mathf.CeilToInt(dm.maxSize / 4f), Mathf.CeilToInt(dm.maxSize / 4f), 1);
    }
    private void ClearData()
    {
        timer = 0;
        smoothPainting = false;
        transform.position = ClampedPosition(InputManager.MousePosition);
        cursorHistory.Clear();
    }
    private Vector3 ClampedPosition(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, ph.boundaryLeft, ph.boundaryRight);
        position.y = Mathf.Clamp(position.y, ph.boundaryDown, ph.boundaryUp);

        return position;
    }
}
