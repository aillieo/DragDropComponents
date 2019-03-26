using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AillieoUtils;

public class TestScript : MonoBehaviour {


    void TargetsOnEnter(DragDropEventData eventData){
        eventData.target.transform.localScale = Vector3.one * 1.2f;
        Debug.Log("targetsOnEnter");
    }

    void TargetsOnExit(DragDropEventData eventData){
        eventData.target.transform.localScale = Vector3.one;
        Debug.Log("targetsOnExit");
    }

    void ItemsOnAttach(DragDropEventData eventData){
        eventData.item.transform.SetParent(eventData.target.targetParent);
        eventData.item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        eventData.item.transform.localScale = Vector3.one;
        Debug.Log("itemsOnAttach");
    }

    void ItemsOnDetach(DragDropEventData eventData){
        eventData.item.transform.localScale = Vector3.one * 1.2f;
        Debug.Log("itemsOnDetach");
    }

    void TargetOnAttach(DragDropEventData eventData){
        eventData.target.transform.localScale = Vector3.one;
        Debug.Log("targetOnAttach");
    }

    void TargetOnDetach(DragDropEventData eventData){
        eventData.item.transform.SetParent(eventData.target.targetParent);
        //eventData.item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Debug.Log("targetOnDetach");
    }

    void ItemsOnEnter(DragDropEventData eventData){
        //eventData.item.transform.localScale = Vector3.one * 1.2f;
        Debug.Log("itemsOnEnter");
    }

    void ItemsOnExit(DragDropEventData eventData){
        //eventData.item.transform.localScale = Vector3.one;
        Debug.Log("itemsOnExit");
    }

    void ItemsOnSetFree(DragDropEventData eventData){
        eventData.item.transform.localScale = Vector3.one;
        eventData.item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Debug.Log("itemsOnSetFree");
    }

    void ItemsOnClick(DragDropEventData eventData){
        Debug.Log("itemsOnClick");
    }


    void Start() {

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
            DragDropTarget target = DragDropHelper.FindComponentUpward<DragDropTarget>(item.transform);
            if (target != null)
            {
                DragDropHelper.InitializePair(item, target);
            }
        }
    }
}
