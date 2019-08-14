using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] ComputeShader HSVcompute;

    [SerializeField] RawImage HUEimage;
    [SerializeField] RawImage SatValimage;
    [SerializeField] Image previewColor;
    [SerializeField] Slider HUESlider;

    [SerializeField] Transform pointer;

    [SerializeField] UIController UIController;

    float h = 0, s = 0, v = 0;
    float boundaryLeft, boundaryRight, boundaryUp, boundaryDown;

    void Awake()
    {
        boundaryLeft = SatValimage.rectTransform.rect.xMin;
        boundaryRight = SatValimage.rectTransform.rect.xMax;
        boundaryDown = SatValimage.rectTransform.rect.yMin;
        boundaryUp = SatValimage.rectTransform.rect.yMax;
        GenerateHue();
        GenerateSatVal();
    }

    public void UpdateSatVal(float hue)
    {
        h = hue;
        HSVcompute.SetFloat("hue", hue);
        HSVcompute.Dispatch(1, Mathf.CeilToInt(512 / 8f), Mathf.CeilToInt(512 / 8f), 1);

        UpdateColor();
    }
    public void MovePointer()
    {
        Vector3 position = SatValimage.transform.InverseTransformPoint(InputManager.MousePosition);
        position.x = Mathf.Clamp(position.x, boundaryLeft, boundaryRight);
        position.y = Mathf.Clamp(position.y, boundaryDown, boundaryUp);
        pointer.localPosition = position;

        s = Mathf.InverseLerp(boundaryLeft, boundaryRight, position.x);
        v = Mathf.InverseLerp(boundaryDown, boundaryUp, position.y);

        UpdateColor();
    }
    public void SetColor(Color colorRGB)
    {
        Color.RGBToHSV(colorRGB, out h, out s, out v);

        HUESlider.SetValueWithoutNotify(h);

        HSVcompute.SetFloat("hue", h);
        HSVcompute.Dispatch(1, Mathf.CeilToInt(512 / 8f), Mathf.CeilToInt(512 / 8f), 1);

        Vector3 position = pointer.localPosition;
        position.x = Mathf.Lerp(boundaryLeft, boundaryRight, s);
        position.y = Mathf.Lerp(boundaryDown, boundaryUp, v);
        pointer.localPosition = position;

        UpdateColor(false);
    }

    private void UpdateColor(bool notify = true)
    {
        previewColor.color = Color.HSVToRGB(h, s, v);
        if (notify) UIController.SetColor(previewColor.color);
    }
    private void GenerateSatVal()
    {
        RenderTexture ttr = RenderTextureCreator.CreateRenderTexture(new Color(0, 0, 0, 0), 512, 512, 0);

        HSVcompute.SetTexture(1, "Result", ttr);
        SatValimage.texture = ttr;

        HSVcompute.SetFloat("hue", h);
        HSVcompute.Dispatch(1, Mathf.CeilToInt(512 / 8f), Mathf.CeilToInt(512 / 8f), 1);
    }
    private void GenerateHue()
    {
        RenderTexture ttr = RenderTextureCreator.CreateRenderTexture(new Color(0, 0, 0, 0), 1, 350, 0);

        HSVcompute.SetTexture(0, "Result", ttr);
        HUEimage.texture = ttr;
        HSVcompute.Dispatch(0, 1, Mathf.CeilToInt(350 / 7f), 1);
    }
}
