using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTartFillingStart : InteractiveImageDragAndMove
{
    public static bool isHoldingFiller = false;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        TartMakingController.instance.ReadyFilling();

        isHoldingFiller = true;
    }

    private void Update()
    {
        if (isHoldingFiller && Input.GetMouseButtonUp(0))
        {
            TartMakingController.instance.EndFillingAndNext();
            isHoldingFiller = false;
            gameObject.SetActive(false);
        }
    }
}
