using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTartFillingStart : InteractiveImageBase
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        TartMakingController.instance.ReadyFilling();
    }
}
