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
        Debug.Log($"{gameObject.name} : Pointer entered the UI element.");
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} : Pointer exited the UI element.");
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} : UI element clicked.");
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} : Pointer down on the UI element.");
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} : Pointer up on the UI element.");
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} : Begin dragging the UI element.");
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} : Dragging the UI element.");
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} : End dragging the UI element.");
    }
}
