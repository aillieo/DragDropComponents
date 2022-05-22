using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AillieoUtils
{
    public static class DragDropHelper
    {

        public static bool IsChannelMatch(DragDropTarget target, DragDropItem item)
        {
            return (target.matchingChannel & item.matchingChannel) != 0;
        }

        public static T FindComponentUpward<T>(Transform trans) where T : Component
        {
            Transform parent = trans.parent;
            T ret = null;
            while (parent)
            {
                ret = parent.GetComponent<T>();
                if (ret)
                {
                    break;
                }
                parent = parent.parent;
            }
            return ret;
        }

        public static void TransferEventToScrollRect(PointerEventData eventData, ScrollRect scrollRect)
        {
            GameObject scrollRectObj = scrollRect.gameObject;
            eventData.pointerEnter = scrollRectObj;
            eventData.pointerPress = scrollRectObj;
            eventData.rawPointerPress = scrollRectObj;
            eventData.pointerDrag = scrollRectObj;
            scrollRect.OnBeginDrag(eventData);
        }

        public static bool TryAddItem(DragDropItem item, DragDropTarget target, bool sendCallback = true)
        {
            if (item == null || target == null)
            {
                return false;
            }
            if (!(item.isActiveAndEnabled && target.isActiveAndEnabled))
            {
                return false;
            }
            if (item.attachedTarget != null)
            {
                return false;
            }
            if (!IsChannelMatch(target, item))
            {
                return false;
            }

            DragDropEventData eventData = new DragDropEventData(!sendCallback);
            eventData.Reset();
            eventData.target = target;
            eventData.item = item;
            target.OnItemAttach(eventData);
            item.OnItemAttach(eventData);
            return true;
        }

        public static bool TryRemoveItem(DragDropItem item, DragDropTarget target, bool sendCallback = true)
        {
            if (item == null || target == null)
            {
                return false;
            }

            if (item.attachedTarget == target && target.HasItemAttached(item))
            {
                DragDropEventData eventData = new DragDropEventData(!sendCallback);
                eventData.Reset();
                eventData.target = target;
                eventData.item = item;
                target.OnItemDetach(eventData);
                item.OnItemDetach(eventData);
                item.OnSetFree(eventData);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int RemoveAllItems(DragDropTarget target, bool sendCallback = true)
        {
            var items = target.GetAllAttachedItems();
            DragDropEventData eventData = new DragDropEventData(!sendCallback);
            eventData.Reset();
            eventData.target = target;
            for (int count = items.Count, i = count - 1; i >= 0; --i)
            {
                DragDropItem item = items[i];
                eventData.item = item;
                target.OnItemDetach(eventData);
                item.OnItemDetach(eventData);
                item.OnSetFree(eventData);
            }
            return items.Count;
        }
    }
}
