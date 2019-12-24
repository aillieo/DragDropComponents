using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace AillieoUtils
{
    [Serializable]
    public class DragDropEvent : UnityEvent<DragDropEventData> { }

    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public abstract class DragDropPair : MonoBehaviour
    {
        // 事件
        [SerializeField]
        protected DragDropEvent m_OnItemExit = new DragDropEvent();
        public DragDropEvent onItemExit { get { return m_OnItemExit; } set { m_OnItemExit = value; } }
        [SerializeField]
        protected DragDropEvent m_OnItemEnter = new DragDropEvent();
        public DragDropEvent onItemEnter { get { return m_OnItemEnter; } set { m_OnItemEnter = value; } }
        [SerializeField]
        protected DragDropEvent m_OnItemDetach = new DragDropEvent();
        public DragDropEvent onItemDetach { get { return m_OnItemDetach; } set { m_OnItemDetach = value; } }
        [SerializeField]
        protected DragDropEvent m_OnItemAttach = new DragDropEvent();
        public DragDropEvent onItemAttach { get { return m_OnItemAttach; } set { m_OnItemAttach = value; } }

        public static readonly int universalMatching = 2147483647; // (2 << 30) - 1

        [SerializeField]
        [Tooltip("用于筛选可匹配的item和target")]
        public int matchingChannel = universalMatching;


        [SerializeField]
        [Tooltip("匹配双方的标记")]
        public int matchingTag;


        private RectTransform m_rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (!m_rectTransform)
                {
                    m_rectTransform = GetComponent<RectTransform>();
                }
                return m_rectTransform;
            }
        }


        protected bool HandleEventForType(DragDropEventTriggerType type, DragDropEventData eventData)
        {

            //Debug.LogError(string.Format("name={0} event={1}",name,type));

            var evt = GetEvent(type);
            if (evt != null)
            {
                evt.Invoke(eventData);
                return true;
            }
            return false;
        }

        public void AddCallback(DragDropEventTriggerType type, UnityAction<DragDropEventData> function)
        {
            var evt = GetEvent(type);
            if (evt != null)
            {
                evt.AddListener(function);
            }
            else
            {
                Debug.LogErrorFormat("Cant find event for type {0}", type);
            }
        }

        public void RemoveCallback(DragDropEventTriggerType type, UnityAction<DragDropEventData> function)
        {
            var evt = GetEvent(type);
            if(evt != null)
            {
                evt.RemoveListener(function);
            }
            else
            {
                Debug.LogErrorFormat("Cant find event for type {0}", type);
            }
        }

        protected virtual DragDropEvent GetEvent(DragDropEventTriggerType type)
        {
            switch (type)
            {
                case DragDropEventTriggerType.ItemExit:
                    return m_OnItemExit;
                case DragDropEventTriggerType.ItemEnter:
                    return m_OnItemEnter;
                case DragDropEventTriggerType.ItemDetach:
                    return m_OnItemDetach;
                case DragDropEventTriggerType.ItemAttach:
                    return m_OnItemAttach;
                default:
                    return null;
            }
        }

        public abstract string GetDebugString();

    }
}
