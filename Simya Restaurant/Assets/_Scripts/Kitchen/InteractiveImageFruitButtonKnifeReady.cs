using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageFruitButtonKnifeReady : InteractiveImageBase
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Cursor.visible = false;
        FruitGatheringController.instance.PickKnife();
        gameObject.SetActive(false);
    }
}
