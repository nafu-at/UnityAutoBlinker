using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NafuSoft.AutoBlinker.Editor
{
    [CustomEditor(typeof(AutoBlinker))]
    public class AutoBlinkerEditor : UnityEditor.Editor
    {
        private AutoBlinker Blinker => target as AutoBlinker;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Undo.RecordObject(target, "Changed Blinker Parameter");
            if (EditorGUILayout.Foldout(true, "Skinned Mesh Renderer"))
            {
                ++EditorGUI.indentLevel;
                Blinker.skinnedMeshRenderer = EditorGUILayout.ObjectField("Skinned Mesh Renderer", Blinker.skinnedMeshRenderer, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;

                Blinker.leftEyeBlinkIndex = EditorGUILayout.Popup("Left Eye BlinkShape", Blinker.leftEyeBlinkIndex + 1, GetBlendShapeArray()) - 1;
                Blinker.rightEyeBlinkIndex = EditorGUILayout.Popup("Right Eye BlinkShape", Blinker.rightEyeBlinkIndex + 1, GetBlendShapeArray()) - 1;
                EditorGUILayout.HelpBox("まばたき用のブレンドシェイプが用意されている場合は片方のみに設定することで動作します", MessageType.Info);
                --EditorGUI.indentLevel;
            }

            if (EditorGUILayout.Foldout(true, "Time Settings"))
            {
                ++EditorGUI.indentLevel;
                Blinker.interval = EditorGUILayout.FloatField("Interval", Blinker.interval);
                Blinker.closeTime = EditorGUILayout.FloatField("Close Time", Blinker.closeTime);
                Blinker.closingTime = EditorGUILayout.FloatField("Closing Time", Blinker.closingTime);
                Blinker.openingTime = EditorGUILayout.FloatField("Opening Time", Blinker.openingTime);
                --EditorGUI.indentLevel;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private string[] GetBlendShapeArray()
        {
            if (Blinker.skinnedMeshRenderer == null)
                return Array.Empty<string>();

            var mesh = Blinker.skinnedMeshRenderer.sharedMesh;
            var shapes = new List<string>
            {
                "None"
            };

            for (var i = 0; i < mesh.blendShapeCount; ++i)
            {
                var shapeName = mesh.GetBlendShapeName(i);
                shapes.Add(shapeName);
            }

            return shapes.ToArray();
        }
    }
}
