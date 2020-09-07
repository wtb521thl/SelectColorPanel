using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public delegate void MouseAction(GameObject go);
    public MouseAction onEnter;
    public MouseAction onExit;
    public MouseAction onClick;
    public MouseAction onDown;
    public MouseAction onUp;
    public MouseAction onDrag;

    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener eventTriggerListener = go.GetComponent<EventTriggerListener>();
        if (eventTriggerListener == null)
        {
            eventTriggerListener = go.AddComponent<EventTriggerListener>();
        }
        return eventTriggerListener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (onClick != null)
        {
            onClick(gameObject);
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (onDown != null)
        {
            onDown(gameObject);
        }
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (onUp != null)
        {
            onUp(gameObject);
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (onEnter != null)
        {
            onEnter(gameObject);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (onExit != null)
        {
            onExit(gameObject);
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (onDrag != null)
        {
            onDrag(gameObject);
        }
    }
}
