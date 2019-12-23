using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AillieoUtils;

public class TestScript : MonoBehaviour {


    public void TargetsOnEnter(DragDropEventData eventData){
        eventData.target.transform.localScale = Vector3.one * 1.2f;
        Debug.Log("targetsOnEnter  " + eventData.ToString());
    }

    public void TargetsOnExit(DragDropEventData eventData){
        eventData.target.transform.localScale = Vector3.one;
        Debug.Log("targetsOnExit  " + eventData.ToString());
    }

    public void ItemsOnAttach(DragDropEventData eventData){
        eventData.item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        eventData.item.transform.localScale = Vector3.one;
        Debug.Log("itemsOnAttach  " + eventData.ToString());
    }

    public void ItemsOnDetach(DragDropEventData eventData){
        eventData.item.transform.localScale = Vector3.one * 1.2f;
        Debug.Log("itemsOnDetach  " + eventData.ToString());
    }

    public void TargetOnAttach(DragDropEventData eventData){
        eventData.target.transform.localScale = Vector3.one;
        Debug.Log("targetOnAttach  " + eventData.ToString());
    }

    public void TargetOnDetach(DragDropEventData eventData){
        Debug.Log("targetOnDetach  " + eventData.ToString());
    }

    public void ItemsOnEnter(DragDropEventData eventData){
        Debug.Log("itemsOnEnter  " + eventData.ToString());
    }

    public void ItemsOnExit(DragDropEventData eventData){
        Debug.Log("itemsOnExit  " + eventData.ToString());
    }

    public void ItemsOnDrag(DragDropEventData eventData)
    {
        Debug.Log("ItemsOnDrag  " + eventData.ToString());
    }

    public void ItemsOnSetFree(DragDropEventData eventData){
        eventData.item.transform.localScale = Vector3.one;
        eventData.item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Debug.Log("itemsOnSetFree  " + eventData.ToString());
    }

    public void ItemsOnClick(DragDropEventData eventData){
        Debug.Log("itemsOnClick  " + eventData.ToString());
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
            item.AddCallback(DragDropEventTriggerType.ItemDrag, ItemsOnDrag);
        }
    }
}
