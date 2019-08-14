using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public delegate void OnValuesChangeDelegate();
    public static event OnValuesChangeDelegate OnShaderDataNeedUpdate;
    public static event OnValuesChangeDelegate OnBrushNeedUpdate;

    [SerializeField] DataManager dm;

    [SerializeField] Button[] brushButtons;

    [SerializeField] Slider powerSlider;
    [SerializeField] Slider lengthSlider;

    [SerializeField] Toggle smoothToggle;
    [SerializeField] Slider speedSlider;
    [SerializeField] Slider weightSlider;

    [SerializeField] Slider frequencySlider;
    [SerializeField] Slider sizeSlider;
    [SerializeField] InputField sizeInput;
    [SerializeField] Slider falloffSlider;

    [SerializeField] ColorPicker colorPicker;

    public void SetBrush(int value)
    {
        UpdateInteractables(value);
        BrushSettings.brushType = (BrushType)value;
    }
    public void SetPower(float value)
    {
        BrushSettings.power = Mathf.Lerp(dm.minPower, dm.maxPower, value);
    }
    public void SetFadeLength(float value)
    {
        BrushSettings.fadeLength = Mathf.Lerp(dm.minFadeLength, dm.maxFadeLength, value);
    }
    public void SetSmooth(bool value)
    {
        BrushSettings.smooth = value;
        speedSlider.interactable = value;
        weightSlider.interactable = value;
    }
    public void SetSpeed(float value)
    {
        BrushSettings.speed = Mathf.Lerp(dm.minSpeed, dm.maxSpeed, value);
    }
    public void SetWeight(float value)
    {
        BrushSettings.weight = Mathf.RoundToInt(Mathf.Lerp(dm.minWeight, dm.maxWeight, value));
    }
    public void SetFrequency(float value)
    {
        BrushSettings.frequency = Mathf.RoundToInt(Mathf.Lerp(dm.minFrequency, dm.maxFrequency, value));
    }
    public void SetSize(string value)
    {
        sizeSlider.value = Mathf.Clamp(int.Parse(value), dm.minSize, dm.maxSize);
    }
    public void SetSize(float value)
    {
        BrushSettings.size = Mathf.RoundToInt(Mathf.Lerp(dm.minSize, dm.maxSize, value));
        sizeInput.SetTextWithoutNotify(BrushSettings.size.ToString());
        OnShaderDataNeedUpdate(); OnBrushNeedUpdate();
    }
    public void SetFalloff(float value)
    {
        BrushSettings.falloff = Mathf.Lerp(dm.minFalloff, dm.maxFalloff, value);
        OnShaderDataNeedUpdate();
    }
    public void SetColor(Color value)
    {
        BrushSettings.color = value;
        OnShaderDataNeedUpdate();
    }

    private void Awake()
    {
        float value;
        dm.InitializeValues();

        UpdateInteractables((int)BrushSettings.brushType, true);

        value = Mathf.InverseLerp(dm.minPower, dm.maxPower, BrushSettings.power);
        powerSlider.SetValueWithoutNotify(value);
        value = Mathf.InverseLerp(dm.minFadeLength, dm.maxFadeLength, BrushSettings.fadeLength);
        lengthSlider.SetValueWithoutNotify(value);

        smoothToggle.SetIsOnWithoutNotify(BrushSettings.smooth);
        speedSlider.interactable = BrushSettings.smooth;
        weightSlider.interactable = BrushSettings.smooth;
        value = Mathf.InverseLerp(dm.minSpeed, dm.maxSpeed, BrushSettings.speed);
        speedSlider.SetValueWithoutNotify(value);
        value = Mathf.InverseLerp(dm.minWeight, dm.maxWeight, BrushSettings.weight);
        weightSlider.SetValueWithoutNotify(value);

        value = Mathf.InverseLerp(dm.minFrequency, dm.maxFrequency, BrushSettings.frequency);
        frequencySlider.SetValueWithoutNotify(value);
        value = Mathf.InverseLerp(dm.minSize, dm.maxSize, BrushSettings.size);
        sizeSlider.SetValueWithoutNotify(value);
        sizeInput.text = BrushSettings.size.ToString();
        value = Mathf.InverseLerp(dm.minFalloff, dm.maxFalloff, BrushSettings.falloff);
        falloffSlider.SetValueWithoutNotify(value);

        colorPicker.SetColor(BrushSettings.color);
    }
    private void Start()
    {
        OnShaderDataNeedUpdate();
        OnBrushNeedUpdate();
    }

    private void UpdateInteractables(int brushType, bool initial = false)
    {
        brushButtons[brushType].interactable = false;
        if (!initial) brushButtons[(int)BrushSettings.brushType].interactable = true;

        if (brushType == (int)BrushType.Brush)
        {
            powerSlider.interactable = true;
            lengthSlider.interactable = true;
        }
        else
        {
            powerSlider.interactable = false;
            lengthSlider.interactable = false;
        }
    }
}
