﻿using UnityEditor;
using UnityEngine;

public class ReadOnlyInInspector : PropertyAttribute
{
 
}
 
[CustomPropertyDrawer(typeof(ReadOnlyInInspector))]
public class ReadOnlyInInspectorDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
        GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
 
    public override void OnGUI(Rect position,
        SerializedProperty property,
        GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}