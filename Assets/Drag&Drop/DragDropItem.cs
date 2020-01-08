using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AillieoUtils
{

    public class DragDropItem : DragDropPair, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler
    {

        // 事件
        [SerializeField]
        protected DragDropEvent m_OnItemSetFree = new DragDropEvent();
        public DragDropEvent onItemSetFree { get { return m_OnItemSetFree; } set { m_OnItemSetFree = value; } }
        [SerializeField]
        protected DragDropEvent m_OnItemClick = new DragDropEvent();
        public DragDropEvent onItemClick { get { return m_OnItemClick; } set { m_OnItemClick = value; } }
        [SerializeField]
        protected DragDropEvent m_OnItemDrag = new DragDropEvent();
        public DragDropEvent onItemDrag { get { return m_OnItemDrag; } set { m_OnItemDrag = value; } }

        [SerializeField]
        [Tooltip("拖动期间的临时父节点")]
        private Transform m_parentWhenDragging;
        public Transform parentWhenDragging
        {
            get
            {
                if (!m_parentWhenDragging)
                {
                    m_parentWhenDragging = DragDropHelper.FindComponentUpward<Canvas>(transform).transform;
                }
                return m_parentWhenDragging;
            }
        }

        [SerializeField]
        [Tooltip("点击x秒后才能算从target上拿起来")]
        private float m_longPressDetach;
        public float longPressDetach
        {
            get
            {
                return m_longPressDetach;
            }
            set
            {
                if (value != m_longPressDetach)
                {
                    m_longPressDetach = value;
                    if (m_longPressDetach > 0)
                    {
                        waitForSeconds = new WaitForSeconds(m_longPressDetach);
                    }
                    else
                    {
                        waitForSeconds = null;
                    }
                }
            }
        }

        public DragDropTarget attachedTarget { get; private set; }

        public DragDropTarget lastTarget { get; private set; }

        bool waitingForDragDropStart = false;

        WaitForSeconds m_waitForSeconds;
        WaitForSeconds waitForSeconds
        {
            get
            {
                if (m_waitForSeconds == null)
                {
                    m_waitForSeconds = new WaitForSeconds(longPressDetach);
                }
                return m_waitForSeconds;
            }
            set
            {
                m_waitForSeconds = value;
            }
        }

        ScrollRect scrollRect = null;

        bool freeItem { get { return attachedTarget == null; } }

        bool delayDetach { get { return longPressDetach > 0; } }

        void OnEnable()
        {
            DragDropRegistry.Instance.UnRegister(this);
        }

        void OnDisable()
        {
            if (!freeItem)
            {
                DragDropHelper.TryRemoveItem(this, this.attachedTarget);
            }
            DragDropRegistry.Instance.UnRegister(this);
        }


        #region 原生事件接口


        public void OnPointerClick(PointerEventData eventData)
        {
            if (DragDropEventData.current.eligibleForClick)
            {
                OnClick(DragDropEventData.current);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (waitingForDragDropStart)
            {
                if (scrollRect)
                {
                    DragDropHelper.TransferEventToScrollRect(eventData, scrollRect);
                }
            }
            else
            {
                if (!delayDetach && scrollRect)
                {
                    bool horizontalDrag = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
                    if (horizontalDrag && !scrollRect.horizontal)
                    {
                        HandleDragDropEvent();
                    }
                    else if (!horizontalDrag && !scrollRect.vertical)
                    {
                        HandleDragDropEvent();
                    }
                    else
                    {
                        DragDropHelper.TransferEventToScrollRect(eventData, scrollRect);
                        return;
                    }
                }

                if (!HandlingDragDropEvent())
                {
                    HandleDragDropEvent();
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!HandlingDragDropEvent())
            {
                return;
            }

            if (rectTransform)
            {
                Vector3 pointerPos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out pointerPos))
                {
                    rectTransform.position = pointerPos;
                }
            }

            OnItemDragged(DragDropEventData.current);

            DragDropTarget newTarget = DragDropRegistry.Instance.FindDropTarget(eventData, this);
            if (newTarget != DragDropEventData.current.target)
            {
                ReplaceDropTarget(DragDropEventData.current.target, newTarget);
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {

            if (!HandlingDragDropEvent())
            {
                DragDropEventData.current.Reset();
                return;
            }

            if (DragDropEventData.current.valid)
            {
                DragDropEventData.current.target.OnItemAttach(DragDropEventData.current);
                OnItemAttach(DragDropEventData.current);
            }
            else
            {
                OnSetFree(DragDropEventData.current);
            }

            DragDropEventData.current.Reset();
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            DragDropEventData.current.Reset();
            scrollRect = DragDropHelper.FindComponentUpward<ScrollRect>(transform);
            DragDropEventData.current.item = this;
            DragDropEventData.current.eligibleForClick = true;

            if (attachedTarget)
            {
                lastTarget = attachedTarget;
            }

            waitingForDragDropStart = false;
            if (delayDetach && !freeItem)
            {
                StartCoroutine(DelayHandleDragDropEvent());
            }
        }

        #endregion 原生事件接口


        #region 拖放事件接口

        public void OnItemDragged(DragDropEventData eventData)
        {
            HandleEventForType(DragDropEventTriggerType.ItemDrag, eventData);
        }

        public void OnItemAttach(DragDropEventData eventData)
        {
            attachedTarget = eventData.target;
            HandleEventForType(DragDropEventTriggerType.ItemAttach, eventData);
        }

        public void OnItemDetach(DragDropEventData eventData)
        {
            attachedTarget = null;
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

        public void OnSetFree(DragDropEventData eventData)
        {
            HandleEventForType(DragDropEventTriggerType.ItemSetFree, eventData);
        }

        public void OnClick(DragDropEventData eventData)
        {
            HandleEventForType(DragDropEventTriggerType.ItemClick, eventData);
        }

        #endregion 拖放事件接口

        protected override DragDropEvent GetEvent(DragDropEventTriggerType type)
        {
            switch (type)
            {
                case DragDropEventTriggerType.ItemSetFree:
                    return m_OnItemSetFree;
                case DragDropEventTriggerType.ItemClick:
                    return m_OnItemClick;
                case DragDropEventTriggerType.ItemDrag:
                    return m_OnItemDrag;
                default:
                    return base.GetEvent(type);
            }
        }

        void HandleDragDropEvent()
        {
            DragDropEventData.current.eligibleForClick = false;
            DragDropEventData.current.eligibleForDrag = true;
            if (!freeItem)
            {
                DragDropEventData.current.target = attachedTarget;
                attachedTarget.OnItemDetach(DragDropEventData.current);
                OnItemDetach(DragDropEventData.current);
                attachedTarget = null;
            }
            transform.SetParent(parentWhenDragging);
        }

        IEnumerator DelayHandleDragDropEvent()
        {
            waitingForDragDropStart = true;
            yield return waitForSeconds;
            waitingForDragDropStart = false;
        }

        bool HandlingDragDropEvent()
        {
            return DragDropEventData.current.eligibleForDrag;
        }

        void ReplaceDropTarget(DragDropTarget oldTarget, DragDropTarget newTarget)
        {
            if (oldTarget == newTarget)
            {
                return;
            }

            if (oldTarget)
            {
                oldTarget.OnItemExit(DragDropEventData.current);
                OnItemExit(DragDropEventData.current);
            }
            DragDropEventData.current.target = newTarget;
            if (newTarget)
            {
                newTarget.OnItemEnter(DragDropEventData.current);
                OnItemEnter(DragDropEventData.current);
            }
        }


        public override string GetDebugString()
        {
            return string.Format("<b>matchingChannel</b> = B{0}\n<b>attachedTarget</b> = {1}\n<b>lastTarget</b> = {2}\n<b>waitingForDragDropStart</b> = {3}\n<b>scrollRect</b> = {4}\n<b>currentEventData</b> = {5}", Convert.ToString(matchingChannel, 2), attachedTarget, lastTarget, waitingForDragDropStart, scrollRect, DragDropEventData.current);
        }

    }

}
