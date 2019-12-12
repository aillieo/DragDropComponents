﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace AillieoUtils
{
    public static class DragDropDrawer
    {
        private static GUIStyle m_labelStyle;
        public static GUIStyle LabelStyle {
            get
            {
                if(m_labelStyle == null)
                {
                    m_labelStyle = new GUIStyle("label") { richText = true, fontStyle = FontStyle.Normal };
                }
                return m_labelStyle;
            }
        }

        public static bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

    }

    [CustomEditor(typeof(DragDropItem))]
    [CanEditMultipleObjects]
    public class DragDropItemEditor : Editor
    {
        bool displayEvents = false;
        DragDropItem dragDropItem;

        SerializedProperty m_OnItemExit;
        SerializedProperty m_OnItemEnter;
        SerializedProperty m_OnItemDetach;
        SerializedProperty m_OnItemAttach;
        SerializedProperty matchingChannel;
        SerializedProperty matchingTag;
        SerializedProperty m_OnItemSetFree;
        SerializedProperty m_OnItemClick;
        SerializedProperty m_parentWhenDragging;
        SerializedProperty m_longPressDetach;
        SerializedProperty m_fallbackTarget;

        private void OnEnable()
        {
            dragDropItem = target as DragDropItem;
            m_OnItemExit = serializedObject.FindProperty("m_OnItemExit");
            m_OnItemEnter = serializedObject.FindProperty("m_OnItemEnter");
            m_OnItemDetach = serializedObject.FindProperty("m_OnItemDetach");
            m_OnItemAttach = serializedObject.FindProperty("m_OnItemAttach");
            matchingChannel = serializedObject.FindProperty("matchingChannel");
            matchingTag = serializedObject.FindProperty("matchingTag");
            m_OnItemSetFree = serializedObject.FindProperty("m_OnItemSetFree");
            m_OnItemClick = serializedObject.FindProperty("m_OnItemClick");
            m_parentWhenDragging = serializedObject.FindProperty("m_parentWhenDragging");
            m_longPressDetach = serializedObject.FindProperty("m_longPressDetach");
            m_fallbackTarget = serializedObject.FindProperty("m_fallbackTarget");
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            EditorGUILayout.PropertyField(matchingChannel, new GUIContent("Matching Channel"));
            EditorGUILayout.PropertyField(matchingTag, new GUIContent("Matching Tag"));
            EditorGUILayout.PropertyField(m_parentWhenDragging, new GUIContent("Parent When Dragging"));
            EditorGUILayout.PropertyField(m_longPressDetach, new GUIContent("Long Press Detach"));
            EditorGUILayout.PropertyField(m_fallbackTarget, new GUIContent("Fallback Target"));

            displayEvents = EditorGUILayout.Foldout(displayEvents, "Serialized DragDropEvents");
            if (displayEvents)
            {
                EditorGUILayout.PropertyField(m_OnItemExit, new GUIContent("On Item Exit"));
                EditorGUILayout.PropertyField(m_OnItemEnter, new GUIContent("On Item Enter"));
                EditorGUILayout.PropertyField(m_OnItemDetach, new GUIContent("On Item Detach"));
                EditorGUILayout.PropertyField(m_OnItemAttach, new GUIContent("On Item Attach"));
                EditorGUILayout.PropertyField(m_OnItemSetFree, new GUIContent("On Item Set Free"));
                EditorGUILayout.PropertyField(m_OnItemClick, new GUIContent("On Item Click"));
            }

            GUILayout.BeginVertical("box");
            GUILayout.Label(dragDropItem.GetDebugString(), DragDropDrawer.LabelStyle);
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedPropertiesWithoutUndo();

        }

        public override bool RequiresConstantRepaint()
        {
            return DragDropDrawer.RequiresConstantRepaint();
        }

    }


    [CustomEditor(typeof(DragDropTarget))]
    [CanEditMultipleObjects]
    public class DragDropTargetEditor : Editor
    {
        bool displayEvents = false;
        DragDropTarget dragDropTarget;

        SerializedProperty m_OnItemExit;
        SerializedProperty m_OnItemEnter;
        SerializedProperty m_OnItemDetach;
        SerializedProperty m_OnItemAttach;
        SerializedProperty matchingChannel;
        SerializedProperty matchingTag;
        SerializedProperty m_maxItemCount;
        SerializedProperty m_replaceItem;

        private void OnEnable()
        {
            dragDropTarget = target as DragDropTarget;

            m_OnItemExit = serializedObject.FindProperty("m_OnItemExit");
            m_OnItemEnter = serializedObject.FindProperty("m_OnItemEnter");
            m_OnItemDetach = serializedObject.FindProperty("m_OnItemDetach");
            m_OnItemAttach = serializedObject.FindProperty("m_OnItemAttach");
            matchingChannel = serializedObject.FindProperty("matchingChannel");
            matchingTag = serializedObject.FindProperty("matchingTag");
            m_maxItemCount = serializedObject.FindProperty("m_maxItemCount");
            m_replaceItem = serializedObject.FindProperty("m_replaceItem");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(matchingChannel, new GUIContent("Matching Channel"));
            EditorGUILayout.PropertyField(matchingTag, new GUIContent("Matching Tag"));
            EditorGUILayout.PropertyField(m_maxItemCount, new GUIContent("Max Item Count"));
            EditorGUILayout.PropertyField(m_replaceItem, new GUIContent("Replace Item"));

            displayEvents = EditorGUILayout.Foldout(displayEvents, "Serialized DragDropEvents");
            if (displayEvents)
            {
                EditorGUILayout.PropertyField(m_OnItemExit, new GUIContent("On Item Exit"));
                EditorGUILayout.PropertyField(m_OnItemEnter, new GUIContent("On Item Enter"));
                EditorGUILayout.PropertyField(m_OnItemDetach, new GUIContent("On Item Detach"));
                EditorGUILayout.PropertyField(m_OnItemAttach, new GUIContent("On Item Attach"));
            }

            GUILayout.BeginVertical("box");
            GUILayout.Label(dragDropTarget.GetDebugString(), DragDropDrawer.LabelStyle);
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public override bool RequiresConstantRepaint()
        {
            return DragDropDrawer.RequiresConstantRepaint();
        }

    }

}

