using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AillieoUtils;

public class TestScript : MonoBehaviour {


    Action<DragDropEventData> targetsOnEnter = (DragDropEventData eventData) => {
        eventData.target.targetParent.localScale = Vector3.one * 1.2f;
        Debug.Log("targetsOnEnter");
    };

    Action<DragDropEventData> targetsOnExit = (DragDropEventData eventData) => {
        eventData.target.targetParent.localScale = Vector3.one;
        Debug.Log("targetsOnExit");
    };

    Action<DragDropEventData> itemsOnAttach = (DragDropEventData eventData) => {
        eventData.item.transform.SetParent(eventData.target.targetParent);
        eventData.item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        eventData.item.transform.localScale = Vector3.one;
        Debug.Log("itemsOnAttach");
    };

    Action<DragDropEventData> itemsOnDetach = (DragDropEventData eventData) => {
        eventData.item.transform.localScale = Vector3.one * 1.2f;
        Debug.Log("itemsOnDetach");
    };

    Action<DragDropEventData> targetOnAttach = (DragDropEventData eventData) => {
        eventData.target.targetParent.localScale = Vector3.one;
        Debug.Log("targetOnAttach");
    };

    Action<DragDropEventData> targetOnDetach = (DragDropEventData eventData) => {
        eventData.item.transform.SetParent(eventData.target.targetParent);
        eventData.item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Debug.Log("targetOnDetach");
    };

    Action<DragDropEventData> itemsOnEnter = (DragDropEventData eventData) => {
        //eventData.item.transform.localScale = Vector3.one * 1.2f;
        Debug.Log("itemsOnEnter");
    };

    Action<DragDropEventData> itemsOnExit = (DragDropEventData eventData) => {
        //eventData.item.transform.localScale = Vector3.one;
        Debug.Log("itemsOnExit");
    };

    Action<DragDropEventData> itemsOnSetFree = (DragDropEventData eventData) => {
        eventData.item.transform.localScale = Vector3.one;
        eventData.item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Debug.Log("itemsOnSetFree");
    };


    void Start () {


        var allTargets = GetComponentsInChildren<DragDropTarget>();
        var allItems = GetComponentsInChildren<DragDropItem>();

        foreach (var target in allTargets)
        {
            target.AddCallback(DragDropEventTriggerType.ItemEnter, targetsOnEnter);
            target.AddCallback(DragDropEventTriggerType.ItemExit, targetsOnExit);
            target.AddCallback(DragDropEventTriggerType.ItemAttach, targetOnAttach);
            target.AddCallback(DragDropEventTriggerType.ItemDetach, targetOnDetach);
        }

        foreach (var item in allItems)
        {
            item.SetTargetsByRootObject(gameObject);
            item.AddCallback(DragDropEventTriggerType.ItemAttach, itemsOnAttach);
            item.AddCallback(DragDropEventTriggerType.ItemDetach, itemsOnDetach);
            item.AddCallback(DragDropEventTriggerType.ItemEnter, itemsOnEnter);
            item.AddCallback(DragDropEventTriggerType.ItemExit, itemsOnExit);
            item.AddCallback(DragDropEventTriggerType.ItemSetFree, itemsOnSetFree);
        }

    }
	
}
