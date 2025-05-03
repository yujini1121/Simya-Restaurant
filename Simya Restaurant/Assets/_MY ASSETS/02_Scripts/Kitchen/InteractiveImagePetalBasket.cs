using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImagePetalBasket : InteractiveImageBase
{
    public static bool isPetalHolding = false;
    public static bool isEntered = false;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        isEntered = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        isEntered = false;
    }
}
