using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTartFillingClickArea : InteractiveImageBase
{
    float startTime;

    public override void OnPointerDown(PointerEventData eventData)
    {
        TartMakingController.instance.StartFilling();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        TartMakingController.instance.EndFillingAndNext();
    }

}
