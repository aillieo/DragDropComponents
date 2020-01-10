using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AillieoUtils;

public class TestScript : MonoBehaviour
{
    public DragDropItem[] avatars;
    public DragDropTarget[] platforms;
    public DragDropTarget pool;

    void Start()
    {
        // Add callbacks for drag-drop items and targets
        var allTargets = GetComponentsInChildren<DragDropTarget>();
        var allItems = GetComponentsInChildren<DragDropItem>();

        foreach (var target in allTargets)
        {
            target.AddCallback(DragDropEventTriggerType.ItemEnter, TargetsOnEnter);
            target.AddCallback(DragDropEventTriggerType.ItemExit, TargetsOnExit);
            target.AddCallback(DragDropEventTriggerType.ItemAttach, TargetOnAttach);
            target.AddCallback(DragDropEventTriggerType.ItemDetach, TargetOnDetach);
        }

        foreach (var item in allItems)
        {
            item.AddCallback(DragDropEventTriggerType.ItemAttach, ItemsOnAttach);
            item.AddCallback(DragDropEventTriggerType.ItemDetach, ItemsOnDetach);
            item.AddCallback(DragDropEventTriggerType.ItemEnter, ItemsOnEnter);
            item.AddCallback(DragDropEventTriggerType.ItemExit, ItemsOnExit);
            item.AddCallback(DragDropEventTriggerType.ItemSetFree, ItemsOnSetFree);
            item.AddCallback(DragDropEventTriggerType.ItemClick, ItemsOnClick);
            item.AddCallback(DragDropEventTriggerType.ItemDrag, ItemsOnDrag);

            DragDropTarget target = DragDropHelper.FindComponentUpward<DragDropTarget>(item.transform);
            if (target != null)
            {
                DragDropHelper.TryAddItem(item, target, false);
            }
        }

        // Init tags & channels
        for(int i = 0; i < avatars.Length; ++ i)
        {
            avatars[i].matchingTag = i;
            avatars[i].matchingChannel = DragDropPair.universalMatching;
        }

        for (int i = 0; i < platforms.Length; ++i)
        {
            platforms[i].matchingTag = i + 1;
            platforms[i].matchingChannel = DragDropPair.universalMatching;
        }

        pool.matchingTag = -1;
        pool.matchingChannel = DragDropPair.universalMatching;

    }

    public void TargetsOnEnter(DragDropEventData eventData)
    {

    }

    public void TargetsOnExit(DragDropEventData eventData)
    {

    }

    public void TargetOnAttach(DragDropEventData eventData)
    {

    }

    public void TargetOnDetach(DragDropEventData eventData)
    {
    
    }

    public void ItemsOnEnter(DragDropEventData eventData)
    {
        if (eventData.target.matchingTag > 0)
        {
            eventData.target.transform.Find("Fx_01").gameObject.SetActive(true);
        }
    }

    public void ItemsOnExit(DragDropEventData eventData)
    {
        if (eventData.target.matchingTag > 0)
        {
            eventData.target.transform.Find("Fx_01").gameObject.SetActive(false);
        }
    }

    public void ItemsOnDrag(DragDropEventData eventData)
    {
        Debug.Log("ItemsOnDrag  " + eventData.ToString());
    }

    public void ItemsOnSetFree(DragDropEventData eventData)
    {
        DragDropHelper.TryAddItem(eventData.item, pool);
    }

    public void ItemsOnClick(DragDropEventData eventData)
    {
        Debug.Log("itemsOnClick  " + eventData.ToString());
    }

    public void ItemsOnAttach(DragDropEventData eventData)
    {
        if (eventData.target.matchingTag > 0)
        {
            if (eventData.target.currentItemCount > 1)
            {
                var item = eventData.target.GetAllAttachedItems()[0];
                var lastTarget = eventData.item.lastTarget;

                DragDropHelper.TryRemoveItem(item, item.attachedTarget, false);

                if (lastTarget != null)
                {
                    DragDropHelper.TryAddItem(item, lastTarget);
                }
            }

            eventData.target.transform.Find("Fx_01").gameObject.SetActive(false);
            eventData.target.transform.Find("Fx_02").gameObject.SetActive(true);

            eventData.item.transform.Find("Avatar").gameObject.SetActive(false);
            eventData.item.transform.Find("Array").gameObject.SetActive(true);
        }
        else
        {
            eventData.item.transform.Find("Avatar").gameObject.SetActive(true);
            eventData.item.transform.Find("Array").gameObject.SetActive(false);
        }

        eventData.item.transform.SetParent(eventData.target.transform, false);
        RectTransform rect = eventData.item.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.one * 0.5f;
        rect.anchorMax = Vector2.one * 0.5f;
        rect.anchoredPosition = Vector2.zero;
    }

    public void ItemsOnDetach(DragDropEventData eventData)
    {
        if (eventData.target.matchingTag > 0)
        {
            eventData.target.transform.Find("Fx_01").gameObject.SetActive(false);
            eventData.target.transform.Find("Fx_02").gameObject.SetActive(false);
        }

        eventData.item.transform.Find("Avatar").gameObject.SetActive(false);
        eventData.item.transform.Find("Array").gameObject.SetActive(true);
    }

}
