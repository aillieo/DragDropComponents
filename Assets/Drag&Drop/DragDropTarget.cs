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
        private readonly List<DragDropItem> attachedItems = new List<DragDropItem>();


        private void OnEnable()
        {
            registerHandle = DragDropRegistry.Instance.Register(this);
        }

        private void OnDisable()
        {
            DragDropHelper.RemoveAllItems(this);
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

        public IList<DragDropItem> GetAllAttachedItems()
        {
            return attachedItems.AsReadOnly();
        }

        public bool HasItemAttached(DragDropItem item)
        {
            return attachedItems.Contains(item);
        }


        StringBuilder sb = new StringBuilder();
        public override string GetDebugString()
        {
            sb.Remove(0, sb.Length);

            for (int i = 0; i < attachedItems.Count; ++ i)
            {
                sb.AppendLine(attachedItems[i].gameObject.name);
            }

            return string.Format("<b>matchingChannel</b> = B{0}\n<b>attachedItems</b> = \n[\n{1}]", Convert.ToString(matchingChannel, 2),sb.ToString());
        }

    }

}
