using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace AillieoUtils
{
    [CustomEditor(typeof(DragDropItem))]
    public class DragDropItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Label(((DragDropItem)target).GetDebugString(), new GUIStyle { richText = true });
        }
    }


    [CustomEditor(typeof(DragDropTarget))]
    public class DragDropTargetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Label(((DragDropTarget)target).GetDebugString(), new GUIStyle { richText = true });
        }
    }
}

