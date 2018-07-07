using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AillieoUtils
{

    public class DragDropItem : DragDropPair, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {

        [Tooltip("拖动期间的临时父节点")]
        public Transform parentWhenDragging;

        [Tooltip("当被拖拽到空白处时 返回上次的target")]
        public bool attachBackWhenSetFree = true;

        [SerializeField]
        [Tooltip("初始状态的附着target")]
        private DragDropTarget m_attachedTarget;
        public DragDropTarget attachedTarget { get { return m_attachedTarget; } private set { m_attachedTarget = value; } }


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
                    if(m_longPressDetach > 0)
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

        bool waitingForDragDropStart = false;


        [SerializeField]
        [Tooltip("如果被挤下来 会放到哪个节点下")]
        private Transform m_fallbackParent;
        public Transform fallbackParent
        {
            get
            {
                if(m_fallbackParent)
                {
                    return m_fallbackParent;
                }
                if(lastParent)
                {
                    return lastParent;
                }
                if(lastTarget)
                {
                    return lastTarget.targetParent;
                }
                return null;
            }

            set
            {
                m_fallbackParent = value;
            }
        }



        DragDropEventData currentEventData = new DragDropEventData();
        DragDropTarget lastTarget = null;
        Transform lastParent = null;

        RectTransform rectTransform;
        Vector3 localPositon;
        Vector2 anchoredPosition;
        WaitForSeconds waitForSeconds = null;

        ScrollRect scrollRect = null;


        // 可能附着的target列表
        List<DragDropTarget> potentialTargets = new List<DragDropTarget>();



        bool freeItem { get { return attachedTarget == null; }}


        bool delayDetach { get { return longPressDetach > 0; }}


        void Start()
        {
            if(!parentWhenDragging)
            {
                parentWhenDragging = transform.parent;
            }

            if (!rectTransform)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            if(!freeItem)
            {
                DragDropHelper.TryAttachItem(this,attachedTarget);
            }

            if(delayDetach)
            {
                waitForSeconds = new WaitForSeconds(longPressDetach);
            }
        }


        public void SetInitialTarget(DragDropTarget target)
        {
            if(!potentialTargets.Contains(target))
            {
                potentialTargets.Add(target);
            }

            if(freeItem)
            {
                attachedTarget = target;
            }

            if(attachedTarget != target)
            {
                Debug.LogError("已经有初始target了");
                return;
            }

            if (attachedTarget.AddInitialAttachedItem(this))
            {
                fallbackParent = attachedTarget.targetParent;
            }
            else
            {
                attachedTarget = null;
            }
        }


        // 获取root 下所有带有DragDropTarget的对象并保存
        List<DragDropTarget> tempList = new List<DragDropTarget>();
        public void SetTargetsByRootObject(GameObject root)
        {
            potentialTargets.Clear();
            if(root)
            {
                tempList.AddRange(root.GetComponentsInChildren<DragDropTarget>());
                //potentialTargets.AddRange(tempList.FindAll((DragDropTarget ddt) => { return DragDropHelper.IsChannelMatch(ddt, this); }));
                potentialTargets.AddRange(tempList);
                tempList.Clear();
            }
        }


        #region 原生事件接口


        public void OnPointerDown(PointerEventData eventData)
        {
            localPositon = transform.localPosition;
            if(rectTransform)
            {
                anchoredPosition = rectTransform.anchoredPosition;
            }

            currentEventData.Reset();
            scrollRect = null;
            currentEventData.item = this;
            currentEventData.eligibleForClick = true;

            if (attachedTarget)
            {
                lastTarget = attachedTarget;
                lastParent = attachedTarget.targetParent;
            }
            else
            {
                lastParent = transform.parent;
            }
 
            if(delayDetach && !freeItem)
            {
                StartCoroutine(DelayHandleDragDropEvent());
            }

        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!waitingForDragDropStart)
            {
                currentEventData.eligibleForClick = false;
                HandleDragDropEvent();
            }

            if (!HandlingDragDropEvent())
            {
                return;
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
                rectTransform.anchoredPosition += eventData.delta;
            }

            DragDropTarget newTarget = FindDropTarget(eventData);
            if (newTarget != currentEventData.target)
            {
                ReplaceDropTarget(currentEventData.target, newTarget);
            }
            
        }



        public void OnPointerUp(PointerEventData eventData)
        {

            if(currentEventData.eligibleForClick)
            {
                OnClick(currentEventData);
            }

            if (!HandlingDragDropEvent())
            {
                return;
            }

            if (currentEventData.valid)
            {
                DragDropItem replacedItem = null;
                if(currentEventData.target.CheckCanAttachItem(currentEventData, out replacedItem))
                {
                    if(replacedItem)
                    {
                        // 旧item 离开
                        currentEventData.isReplaced = true;
                        currentEventData.item = replacedItem;
                        currentEventData.target.OnItemDetach(currentEventData);
                        replacedItem.OnItemDetach(currentEventData);

                        // 新item 附着
                        currentEventData.isReplaced = false;
                        currentEventData.item = this;
                        currentEventData.target.OnItemAttach(currentEventData);
                        OnItemAttach(currentEventData);

                        // 旧item 附着到新item原来的target 完成交换
                        currentEventData.isReplaced = true;
                        currentEventData.item = replacedItem;
                        currentEventData.target = lastTarget;
                        if (currentEventData.valid)
                        {
                            lastTarget.OnItemAttach(currentEventData);
                            replacedItem.OnItemAttach(currentEventData);
                        }
                        else
                        {
                            replacedItem.attachedTarget = null;
                            replacedItem.OnSetFree(currentEventData);
                        }
                    }
                    else
                    {
                        currentEventData.target.OnItemAttach(currentEventData);
                        OnItemAttach(currentEventData);
                    }

                }
                else
                {
                    currentEventData.target.OnItemExit(currentEventData);
                    OnItemExit(currentEventData);
                }
            }
            else
            {
                OnSetFree(currentEventData);
            }


            currentEventData.Reset();

        }


        public void OnEndDrag(PointerEventData eventData)
        {
            if (!HandlingDragDropEvent())
            {
                return;
            }
        }


        #endregion 原生事件接口


        #region 拖放事件接口


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
            HandleEventForType(DragDropEventTriggerType.ItemExit, currentEventData);
        }

        public void OnItemEnter(DragDropEventData eventData)
        {
            HandleEventForType(DragDropEventTriggerType.ItemEnter, eventData);
        }

        public void OnSetFree(DragDropEventData eventData)
        {
            if(lastTarget && attachBackWhenSetFree)
            {
                eventData.target = lastTarget;
                eventData.target.OnItemAttach(eventData);
                OnItemAttach(eventData);
            }
            else
            {
                if (fallbackParent)
                {
                    transform.SetParent(fallbackParent, false);
                }
                HandleEventForType(DragDropEventTriggerType.ItemSetFree, eventData);
            }
        }

        public void OnClick(DragDropEventData eventData)
        {
            HandleEventForType(DragDropEventTriggerType.ItemClick, eventData);
        }

        #endregion 拖放事件接口


        void HandleDragDropEvent()
        {
            currentEventData.eligibleForClick = false;
            currentEventData.eligibleForDrag = true;
            if (!freeItem)
            {
                currentEventData.target = attachedTarget;
                attachedTarget.OnItemDetach(currentEventData);
                OnItemDetach(currentEventData);
                attachedTarget = null;
            }
            transform.SetParent(parentWhenDragging);
        }

        IEnumerator DelayHandleDragDropEvent()
        {
            waitingForDragDropStart = true;
            yield return waitForSeconds;
            HandleDragDropEvent();
        }

        bool HandlingDragDropEvent()
        {
            if(currentEventData.eligibleForDrag)
            {
                return true;
            }
            else
            {
                if(waitingForDragDropStart)
                {
                    waitingForDragDropStart = false;
                    StopAllCoroutines();
                    currentEventData.Reset();
                }
                return false;
            }
        }

        DragDropTarget FindDropTarget(PointerEventData pointerEventData)
        {
            foreach (var ddt in potentialTargets)
            {
                if(!DragDropHelper.IsChannelMatch(ddt,this))
                {
                    continue;
                }
                bool contain = RectTransformUtility.RectangleContainsScreenPoint(ddt.rectTransform, 
                    pointerEventData.position, 
                    pointerEventData.pressEventCamera);
                if(contain)
                {
                    return ddt;
                }
            }
            return null;
        }


        void ReplaceDropTarget(DragDropTarget oldTarget , DragDropTarget newTarget)
        {
            if(oldTarget == newTarget)
            {
                return;
            }

            if(oldTarget)
            {
                oldTarget.OnItemExit(currentEventData);
                OnItemExit(currentEventData);
            }
            currentEventData.target = newTarget;
            if(newTarget)
            {
                newTarget.OnItemEnter(currentEventData);
                OnItemEnter(currentEventData);
            }
        }


        public string GetDebugString()
        {
#if UNITY_EDITOR
            return string.Format("<b>lastParent</b> = {0}\n<b>lastTarget</b> = {1}\n<b>rectTransform</b> = {2}\n<b>currentEventData</b> = {3}", lastParent,lastTarget,rectTransform,currentEventData);
#endif
            return "";
        }

    }

}