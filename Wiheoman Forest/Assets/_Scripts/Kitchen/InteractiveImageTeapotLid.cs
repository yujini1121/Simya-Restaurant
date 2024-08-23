using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTeapotLid : InteractiveImageBase
{
    public static bool isEntered = false;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        isEntered = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        isEntered = false;
    }

    private void Start()
    {
        GetComponent<UnityEngine.UI.Image>().color = Color.clear;
    }
}
