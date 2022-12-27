using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NafuSoft.AutoBlinker.Editor
{
    [CustomEditor(typeof(Blinker))]
    public class BlinkerCustomEditor : UnityEditor.Editor
    {
        private Blinker _blinker;

        private void OnEnable()
        {
            _blinker = target as Blinker;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Undo.RecordObject(target, "Changed Blinker Parameter");
            if (EditorGUILayout.Foldout(true, "Skinned Mesh Renderer"))
            {
                ++EditorGUI.indentLevel;
                _blinker.skinnedMeshRenderer = EditorGUILayout.ObjectField("Skinned Mesh Renderer", _blinker.skinnedMeshRenderer, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;

                _blinker.leftEyeBlinkIndex = EditorGUILayout.Popup("Left Eye BlinkShape", _blinker.leftEyeBlinkIndex + 1, GetBlendShapeArray()) - 1;
                _blinker.rightEyeBlinkIndex = EditorGUILayout.Popup("Right Eye BlinkShape", _blinker.rightEyeBlinkIndex + 1, GetBlendShapeArray()) - 1;
                EditorGUILayout.HelpBox("まばたき用のブレンドシェイプが用意されている場合は片方のみに設定することで動作します", MessageType.Info);
                --EditorGUI.indentLevel;
            }

            if (EditorGUILayout.Foldout(true, "Time Settings"))
            {
                ++EditorGUI.indentLevel;
                _blinker.interval = EditorGUILayout.FloatField("Interval", _blinker.interval);
                _blinker.closeTime = EditorGUILayout.FloatField("Close Time", _blinker.closeTime);
                _blinker.closingTime = EditorGUILayout.FloatField("Closing Time", _blinker.closingTime);
                _blinker.openingTime = EditorGUILayout.FloatField("Opening Time", _blinker.openingTime);
                --EditorGUI.indentLevel;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private string[] GetBlendShapeArray()
        {
            if (_blinker.skinnedMeshRenderer == null)
                return Array.Empty<string>();

            var mesh = _blinker.skinnedMeshRenderer.sharedMesh;
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
