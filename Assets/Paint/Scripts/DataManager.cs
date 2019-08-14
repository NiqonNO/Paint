using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class DataManager : MonoBehaviour
{
    [SerializeField] BrushType startBrushType = BrushType.Pencil;
    [SerializeField] bool startSmooth = true;
    [SerializeField] Color startColor = Color.black;

    [SerializeField] float startPower = 5f;
    public float minPower = 10; // Inverted becouse more = less power
    public float maxPower = 1; // Inverted becouse more = less power
    [SerializeField] float startFadeLength = 100f;
    public float minFadeLength = 10;
    public float maxFadeLength = 1000;

    [SerializeField] float startSpeed = 0.1f;
    public float minSpeed = 0.01f;
    public float maxSpeed = 0.2f;
    [SerializeField] int startWeight = 20;
    public int minWeight = 3;
    public int maxWeight = 50;

    [SerializeField] int startFrequency = 1;
    public int minFrequency = 100; // Inverted becouse more = more distance between paints
    public int maxFrequency = 1; // Inverted becouse more = more distance between paints
    [SerializeField] int startSize = 10;
    public int minSize = 1;
    public int maxSize = 250;
    [SerializeField] float startFalloff = 0.5f;
    public float minFalloff = 0;
    public float maxFalloff = 0.99f; // Less than 1 to avoid dividing by 0 in shader

    private void OnValidate()
    {
        startPower = Mathf.Clamp((startPower), maxPower, minPower);
        maxPower = Mathf.Clamp((maxPower), 0, minPower);
        minPower = Mathf.Max(maxPower, (minPower));
        startFadeLength = Mathf.Clamp((startFadeLength), minFadeLength, maxFadeLength);
        minFadeLength = Mathf.Clamp((minFadeLength), 1, maxFadeLength);
        maxFadeLength = Mathf.Max(minFadeLength, (maxFadeLength));
        startSpeed = Mathf.Clamp((startSpeed), minSpeed, maxSpeed);
        minSpeed = Mathf.Clamp((minSpeed), 0.001f, maxSpeed);
        maxSpeed = Mathf.Max(minSpeed, (maxSpeed));
        startWeight = Mathf.Clamp((startWeight), minWeight, maxWeight);
        minWeight = Mathf.Clamp((minWeight), 3, maxWeight);
        maxWeight = Mathf.Max(minWeight, (maxWeight));
        startFrequency = Mathf.Clamp((startFrequency), maxFrequency, minFrequency);
        maxFrequency = Mathf.Clamp((maxFrequency), 1, minFrequency);
        minFrequency = Mathf.Max(maxFrequency, (minFrequency));
        startSize = Mathf.Clamp((startSize), minSize, maxSize);
        minSize = Mathf.Clamp((minSize), 1, maxSize);
        maxSize = Mathf.Max(minSize, (maxSize));
        startFalloff = Mathf.Clamp((startFalloff), minFalloff, maxFalloff);
    }

    public void InitializeValues()
    {
        BrushSettings.brushType = startBrushType;
        BrushSettings.smooth = startSmooth;
        BrushSettings.speed = Mathf.Clamp(startSpeed, minSpeed, maxSpeed);
        BrushSettings.weight = Mathf.Clamp(startWeight, minWeight, maxWeight);

        BrushSettings.fadeLength = Mathf.Clamp(startFadeLength, minFadeLength, maxFadeLength);
        BrushSettings.power = Mathf.Clamp(startPower, maxPower, minPower); // Remember to invert

        BrushSettings.frequency = Mathf.Clamp(startFrequency, maxFrequency, minFrequency); // Remember to invert
        BrushSettings.size = Mathf.Clamp(startSize, minSize, maxSize);
        BrushSettings.falloff = Mathf.Clamp(startFalloff, minFalloff, maxFalloff);
        BrushSettings.color = startColor;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        float inspectorWidthQuater = Screen.width / 4;
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startBrushType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startSmooth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startColor"));
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal(EditorStyles.numberField);
        GUILayout.Label("value");
        GUILayout.Label("start");
        GUILayout.Label("min");
        GUILayout.Label("max");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Power", GUILayout.Width(inspectorWidthQuater)); // Inverted min/max for less confusing inspector edit
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startPower"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxPower"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minPower"), GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Fade Length", GUILayout.Width(inspectorWidthQuater));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startFadeLength"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minFadeLength"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxFadeLength"), GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Speed", GUILayout.Width(inspectorWidthQuater));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startSpeed"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minSpeed"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxSpeed"), GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Wight", GUILayout.Width(inspectorWidthQuater));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startWeight"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minWeight"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxWeight"), GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Frequency", GUILayout.Width(inspectorWidthQuater)); // Inverted min/max for less confusing inspector edit
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startFrequency"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxFrequency"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minFrequency"), GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Size", GUILayout.Width(inspectorWidthQuater));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startSize"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minSize"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxSize"), GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Start Falloff", GUILayout.Width(inspectorWidthQuater));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("startFalloff"), GUIContent.none);
        GUI.enabled = false; // those values dont need to change
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minFalloff"), GUIContent.none);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxFalloff"), GUIContent.none);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif