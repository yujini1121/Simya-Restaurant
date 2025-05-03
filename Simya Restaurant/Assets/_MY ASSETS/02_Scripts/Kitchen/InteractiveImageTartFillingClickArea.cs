using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTartFillingClickArea : InteractiveImageBase
{
    float startTime;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (InteractiveImageTartFillingStart.isHoldingFiller)
        {
            TartMakingController.instance.StartFilling();
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (InteractiveImageTartFillingStart.isHoldingFiller)
        {
            TartMakingController.instance.PauseFilling();
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        TartMakingController.instance.EndFillingAndNext();
    }

}
