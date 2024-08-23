using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageBase : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered the UI element.");
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited the UI element.");
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI element clicked.");
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down on the UI element.");
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer up on the UI element.");
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin dragging the UI element.");
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging the UI element.");
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End dragging the UI element.");
    }
}
