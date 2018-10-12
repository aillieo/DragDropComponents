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
            if(target.universalMatching || item.universalMatching)
            {
                return true;
            }
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

        public static bool InitializePair(DragDropItem item, DragDropTarget target)
        {
            item.SetInitialTarget(target);
            return true;
        }


        public static bool TryRemoveItem(DragDropItem item, DragDropTarget target)
        {
            if(item.attachedTarget == target && target.HasItemAttached(item))
            {
                DragDropEventData eventData = new DragDropEventData();
                eventData.Reset();
                eventData.external = true;
                eventData.target = target;
                eventData.item = item;
                target.OnItemDetach(eventData);
                item.OnItemDetach(eventData);
                target.OnItemExit(eventData);
                item.OnItemExit(eventData);
                item.OnSetFree(eventData);
                return true;
            }
            else
            {
                return false;
            }
        }


        public static int RemoveAllItems(DragDropTarget target)
        {
            var items = target.GetAllAttachedItems();
            DragDropEventData eventData = new DragDropEventData();
            eventData.Reset();
            eventData.external = true;
            eventData.target = target;
            foreach (var item in items)
            {
                eventData.item = item;
                target.OnItemDetach(eventData);
                item.OnItemDetach(eventData);
                target.OnItemExit(eventData);
                item.OnItemExit(eventData);
                item.OnSetFree(eventData);
            }
            return items.Length;
        }
    }

}