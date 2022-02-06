using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Blinker))]
public class BlinkerCustomEditor : Editor
{
    private Blinker blinker;

    private void OnEnable()
    {
        blinker = target as Blinker;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Undo.RecordObject(target, "Changed Blinker Parameter");
        if (EditorGUILayout.Foldout(true, "Skinned Mesh Renderer"))
        {
            ++EditorGUI.indentLevel;
            blinker.skinnedMeshRenderer = EditorGUILayout.ObjectField("Skinned Mesh Renderer", blinker.skinnedMeshRenderer, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;

            blinker.leftEyeBlinkIndex = EditorGUILayout.Popup("Left Eye BlinkShape", blinker.leftEyeBlinkIndex + 1, GetBlendShapeArray()) - 1;
            blinker.rightEyeBlinkIndex = EditorGUILayout.Popup("Right Eye BlinkShape", blinker.rightEyeBlinkIndex + 1, GetBlendShapeArray()) - 1;
            EditorGUILayout.HelpBox("まばたき用のブレンドシェイプが用意されている場合は片方のみに設定することで動作します", MessageType.Info);
            --EditorGUI.indentLevel;
        }

        if (EditorGUILayout.Foldout(true, "Time Settings"))
        {
            ++EditorGUI.indentLevel;
            blinker.interval = EditorGUILayout.FloatField("Interval", blinker.interval);
            blinker.closingTime = EditorGUILayout.FloatField("Closing Time", blinker.closingTime);
            blinker.openingTime = EditorGUILayout.FloatField("Opening Time", blinker.openingTime);
            blinker.closeTime = EditorGUILayout.FloatField("Close Time", blinker.closeTime);
            --EditorGUI.indentLevel;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private string[] GetBlendShapeArray()
    {
        if (blinker.skinnedMeshRenderer == null)
            return Array.Empty<string>();

        var mesh = blinker.skinnedMeshRenderer.sharedMesh;
        var names = new List<string>
        {
            "None"
        };

        for (var i = 0; i < mesh.blendShapeCount; ++i)
        {
            var name = mesh.GetBlendShapeName(i);
            names.Add(name);
        }

        return names.ToArray();
    }
}
