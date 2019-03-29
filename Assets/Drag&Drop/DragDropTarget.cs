using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;
using System;

namespace AillieoUtils
{

    public class DragDropTarget : DragDropPair
    {

        [SerializeField]
        [Tooltip("最多可以附着多少个item")]
        private int m_maxItemCount = 1;
        public int maxItemCount { get { return m_maxItemCount; } private set { m_maxItemCount = value; } }


        [SerializeField]
        [Tooltip("当超出可容纳的item时 是否踢掉最早挂上来的")]
        private bool m_replaceItem;
        public bool replaceItem { get { return m_replaceItem; } }

        public Transform targetParent { get { return transform; } }

        private LinkedListNode<DragDropTarget> registerHandle;


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


        private void OnEnable()
        {
            maxItemCount = Mathf.Max(maxItemCount,1);
            registerHandle = DragDropRegistry.Instance.Register(this);
        }

        private void OnDisable()
        {
            DragDropRegistry.Instance.UnRegister(registerHandle);
            registerHandle = null;
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


        internal bool CheckCanAttachItem(DragDropEventData eventData, out DragDropItem replacedItem)
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

        public DragDropItem[] GetAllAttachedItems()
        {
            return attachedItems.ToArray();
        }


        public bool HasItemAttached(DragDropItem item)
        {
            return attachedItems.Contains(item);
        }


        StringBuilder sb = new StringBuilder();
        public override string GetDebugString()
        {
#if UNITY_EDITOR
            sb.Remove(0, sb.Length);

            for (int i = 0; i < attachedItems.Count; ++ i)
            {
                sb.AppendLine(attachedItems[i].gameObject.name);
            }

            return string.Format("<b>matchingChannel</b> = B{0}\n<b>attachedItems</b> = \n[\n{1}]", Convert.ToString(matchingChannel, 2),sb.ToString());
#else
            return "";
#endif
        }

    }

}