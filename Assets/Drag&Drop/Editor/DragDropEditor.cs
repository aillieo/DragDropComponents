using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace AillieoUtils
{
    [CustomEditor(typeof(DragDropPair),true)]
    [CanEditMultipleObjects]
    public class DragDropItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.BeginVertical("box");
            GUILayout.Label(((DragDropPair)target).GetDebugString(), new GUIStyle { richText = true });
            GUILayout.EndVertical();
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

    }

}

