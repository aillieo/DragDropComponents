using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AillieoUtils
{
    public class DragDropRegistry // : Singleton<DragDropRegistry>
    {
        private static DragDropRegistry m_instance = null;
        private DragDropRegistry() { }

        public static DragDropRegistry Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new DragDropRegistry();
                }

                return m_instance;
            }
        }

        public static void Destroy()
        {
            m_instance = null;
        }

        readonly LinkedList<DragDropTarget> managedTargets = new LinkedList<DragDropTarget>();

        readonly HashSet<DragDropItem> managedItems = new HashSet<DragDropItem>();

        public LinkedListNode<DragDropTarget> Register(DragDropTarget target)
        {
            return managedTargets.AddLast(target);
        }

        public void UnRegister(LinkedListNode<DragDropTarget> registerHandle)
        {
            managedTargets.Remove(registerHandle);
        }

        public void Register(DragDropItem item)
        {
            managedItems.Add(item);
        }

        public void UnRegister(DragDropItem item)
        {
            managedItems.Remove(item);
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
