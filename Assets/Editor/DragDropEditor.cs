using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

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

        private static int channelMin = 0;
        private static int channelMax = 31;
        private static string[] channels = Enumerable.Range(channelMin, channelMax).Select(i => i.ToString()).ToArray();
        public static void DrawMatchingChannel(SerializedProperty serializedProperty, GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal();

            //EditorGUILayout.LabelField(label);

            EditorGUILayout.PropertyField(serializedProperty, label);

            int channel = serializedProperty.intValue;

            int oldMask = 0;
            for (int i = channelMin; i <= channelMax; ++i)
            {
                if ((channel & (1 << i)) != 0)
                {
                    oldMask |= (1 << i);
                }
            }

            int newMask = EditorGUILayout.MaskField(GUIContent.none, oldMask, channels, options);
            if (newMask != oldMask)
            {
                int newChannelValue = 0;
                if (newMask != -1)
                {
                    for (int i = channelMin; i <= channelMax; ++i)
                    {
                        if ((newMask & (1 << i)) != 0)
                        {
                            newChannelValue |= (1 << i);
                        }
                    }
                }
                else
                {
                    newChannelValue = -1;
                }

                channel = newChannelValue;
            }

            if (channel != serializedProperty.intValue)
            {
                serializedProperty.intValue = channel;
            }

            EditorGUILayout.EndHorizontal();

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
        SerializedProperty m_OnItemDrag;
        SerializedProperty matchingChannel;
        SerializedProperty matchingTag;
        SerializedProperty m_OnItemSetFree;
        SerializedProperty m_OnItemClick;
        SerializedProperty m_parentWhenDragging;
        SerializedProperty m_longPressDetach;

        private void OnEnable()
        {
            dragDropItem = target as DragDropItem;
            m_OnItemExit = serializedObject.FindProperty("m_OnItemExit");
            m_OnItemEnter = serializedObject.FindProperty("m_OnItemEnter");
            m_OnItemDetach = serializedObject.FindProperty("m_OnItemDetach");
            m_OnItemAttach = serializedObject.FindProperty("m_OnItemAttach");
            m_OnItemDrag = serializedObject.FindProperty("m_OnItemDrag");
            matchingChannel = serializedObject.FindProperty("matchingChannel");
            matchingTag = serializedObject.FindProperty("matchingTag");
            m_OnItemSetFree = serializedObject.FindProperty("m_OnItemSetFree");
            m_OnItemClick = serializedObject.FindProperty("m_OnItemClick");
            m_parentWhenDragging = serializedObject.FindProperty("m_parentWhenDragging");
            m_longPressDetach = serializedObject.FindProperty("m_longPressDetach");
            displayEvents = !Application.isPlaying;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            DragDropDrawer.DrawMatchingChannel(matchingChannel, new GUIContent("Matching Channel"));
            EditorGUILayout.PropertyField(matchingTag, new GUIContent("Matching Tag"));
            EditorGUILayout.PropertyField(m_parentWhenDragging, new GUIContent("Parent When Dragging"));
            EditorGUILayout.PropertyField(m_longPressDetach, new GUIContent("Long Press Detach"));

            displayEvents = EditorGUILayout.Foldout(displayEvents, "Serialized DragDropEvents");
            if (displayEvents)
            {
                EditorGUILayout.PropertyField(m_OnItemExit, new GUIContent("On Item Exit"));
                EditorGUILayout.PropertyField(m_OnItemEnter, new GUIContent("On Item Enter"));
                EditorGUILayout.PropertyField(m_OnItemDetach, new GUIContent("On Item Detach"));
                EditorGUILayout.PropertyField(m_OnItemAttach, new GUIContent("On Item Attach"));
                EditorGUILayout.PropertyField(m_OnItemSetFree, new GUIContent("On Item Set Free"));
                EditorGUILayout.PropertyField(m_OnItemClick, new GUIContent("On Item Click"));
                EditorGUILayout.PropertyField(m_OnItemDrag, new GUIContent("On Item Drag"));
            }

            if (Application.isPlaying)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label(dragDropItem.GetDebugString(), DragDropDrawer.LabelStyle);
                GUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();

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

        private void OnEnable()
        {
            dragDropTarget = target as DragDropTarget;

            m_OnItemExit = serializedObject.FindProperty("m_OnItemExit");
            m_OnItemEnter = serializedObject.FindProperty("m_OnItemEnter");
            m_OnItemDetach = serializedObject.FindProperty("m_OnItemDetach");
            m_OnItemAttach = serializedObject.FindProperty("m_OnItemAttach");
            matchingChannel = serializedObject.FindProperty("matchingChannel");
            matchingTag = serializedObject.FindProperty("matchingTag");
            displayEvents = !Application.isPlaying;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DragDropDrawer.DrawMatchingChannel(matchingChannel, new GUIContent("Matching Channel"));
            EditorGUILayout.PropertyField(matchingTag, new GUIContent("Matching Tag"));

            displayEvents = EditorGUILayout.Foldout(displayEvents, "Serialized DragDropEvents");
            if (displayEvents)
            {
                EditorGUILayout.PropertyField(m_OnItemExit, new GUIContent("On Item Exit"));
                EditorGUILayout.PropertyField(m_OnItemEnter, new GUIContent("On Item Enter"));
                EditorGUILayout.PropertyField(m_OnItemDetach, new GUIContent("On Item Detach"));
                EditorGUILayout.PropertyField(m_OnItemAttach, new GUIContent("On Item Attach"));
            }

            if(Application.isPlaying)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label(dragDropTarget.GetDebugString(), DragDropDrawer.LabelStyle);
                GUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override bool RequiresConstantRepaint()
        {
            return DragDropDrawer.RequiresConstantRepaint();
        }

    }

}

