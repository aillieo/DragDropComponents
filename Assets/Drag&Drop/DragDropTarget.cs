using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;

namespace AillieoUtils
{

    public class DragDropTarget : DragDropPair
    {

        [SerializeField]
        [Tooltip("当有item附着到此target时 会以此为父节点")]
        private Transform m_targetParent;
        public Transform targetParent { get { return m_targetParent; } private set { m_targetParent = value; } }


        [SerializeField]
        [Tooltip("最多可以附着多少个item")]
        private int m_maxItemCount = 1;
        public int maxItemCount { get { return m_maxItemCount; } private set { m_maxItemCount = value; } }


        [SerializeField]
        [Tooltip("用于标记此target的范围")]
        private RectTransform m_rectTransform;
        public RectTransform rectTransform { get { return m_rectTransform; } private set { m_rectTransform = value; } }


        [SerializeField]
        [Tooltip("当超出可容纳的item时 是否踢掉最早挂上来的")]
        private bool m_replaceItem;
        public bool replaceItem { get { return m_replaceItem; } }


        // 当前附着了多少个item
        public int currentItemCount
        {
            get
            {
                return attachedItems.Count;
            }
        }

        // 保存当前附着的item 按照附着时间先后顺序
        List<DragDropItem> attachedItems = new List<DragDropItem>();


        void Start()
        {
            if (!targetParent)
            {
                targetParent = transform;
            }

            if (!rectTransform)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            if(maxItemCount < 1)
            {
                maxItemCount = 1;
            }
        }

        #region 拖放事件接口

        public void OnItemDetach(DragDropEventData eventData)
        {
            attachedItems.Remove(eventData.item);
            HandleEventForType(DragDropEventTriggerType.ItemDetach, eventData);
        }

        public void OnItemExit(DragDropEventData eventData)
        {
            HandleEventForType(DragDropEventTriggerType.ItemExit, eventData);
        }

        public void OnItemEnter(DragDropEventData eventData)
        {
            HandleEventForType(DragDropEventTriggerType.ItemEnter, eventData);
        }

        public void OnItemAttach(DragDropEventData eventData)
        {
            HandleEventForType(DragDropEventTriggerType.ItemAttach, eventData);
            attachedItems.Add(eventData.item);
        }

        #endregion 拖放事件接口


        public bool CheckCanAttachItem(DragDropEventData eventData, out DragDropItem replacedItem)
        {
            replacedItem = null;

            if (attachedItems.Count >= maxItemCount)
            {
                if(replaceItem)
                {
                    replacedItem = attachedItems[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public bool AddInitialAttachedItem(DragDropItem item)
        {
            bool contain = attachedItems.Contains(item);

            if (attachedItems.Count >= maxItemCount && !contain)
            {
                return false;
            }

            if(!contain)
            {
                attachedItems.Add(item);
            }
            return true;
        }

        public DragDropItem[] GetAllAttachedItems()
        {
            return attachedItems.ToArray();
        }


        public bool HasItemAttached(DragDropItem item)
        {
            return attachedItems.Contains(item);
        }


        StringBuilder sb = new StringBuilder();
        public string GetDebugString()
        {
#if UNITY_EDITOR
            sb.Remove(0, sb.Length);

            for (int i = 0; i < attachedItems.Count; ++ i)
            {
                sb.Append(attachedItems[i].gameObject.name);
                if(i != attachedItems.Count - 1)
                {
                    sb.Append(",");
                }
            }

            return string.Format("<b>attachedItems</b> = \n[{0}]",sb.ToString());
#endif
            return "";
        }

    }

}