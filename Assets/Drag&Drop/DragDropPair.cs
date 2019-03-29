using System.Collections.Generic;
using UnityEngine;
using System;

namespace AillieoUtils
{

    public delegate void DragDropEvent(DragDropEventData dragDropEventData);


    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public abstract class DragDropPair : MonoBehaviour
    {

        protected Dictionary<DragDropEventTriggerType, DragDropEvent> callbacks = new Dictionary<DragDropEventTriggerType, DragDropEvent>();


        [SerializeField]
        [Tooltip("用于筛选可匹配的item和target")]
        public int matchingChannel;


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

            bool handleEvent = false;
            if (callbacks.ContainsKey(type))
            {
                callbacks[type](eventData);
                handleEvent = true;
            }
            return handleEvent;
        }

        public void AddCallback(DragDropEventTriggerType type, DragDropEvent function)
        {
            if (callbacks.ContainsKey(type))
            {
                callbacks[type] += function;
            }
            callbacks[type] = function;
        }

        public void RemoveCallback(DragDropEventTriggerType type, DragDropEvent function)
        {
            if (callbacks.ContainsKey(type))
            {
                callbacks[type] -= function;
                if(callbacks[type].GetInvocationList().Length == 0)
                {
                    callbacks.Remove(type);
                }
            }
        }

        public void OnDestroy()
        {
            callbacks.Clear();
        }

        public abstract string GetDebugString();

    }
}
