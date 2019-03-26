using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AillieoUtils
{
    public class DragDropRegistry : Singleton<DragDropRegistry>
    {
        LinkedList<DragDropTarget> managedTargets = new LinkedList<DragDropTarget>();

        public LinkedListNode<DragDropTarget> Register(DragDropTarget target)
        {
            return managedTargets.AddLast(target);
        }

        public void UnRegister(LinkedListNode<DragDropTarget> registerHandle)
        {
            managedTargets.Remove(registerHandle);
        }
        
        public DragDropTarget FindDropTarget(PointerEventData pointerEventData, DragDropItem dragDropItem)
        {
            foreach (var ddt in managedTargets)
            {
                if (!ddt.isActiveAndEnabled)
                {
                    continue;
                }
                if (!DragDropHelper.IsChannelMatch(ddt, dragDropItem))
                {
                    continue;
                }
                bool contain = RectTransformUtility.RectangleContainsScreenPoint(ddt.rectTransform,
                    pointerEventData.position,
                    pointerEventData.pressEventCamera);
                if (contain)
                {
                    return ddt;
                }
            }
            return null;
        }

    }
}
