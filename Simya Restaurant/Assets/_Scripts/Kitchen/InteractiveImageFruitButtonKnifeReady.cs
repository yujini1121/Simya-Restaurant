using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageFruitButtonKnifeReady : InteractiveImageBase
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        FruitGatheringController.instance.PickKnife();
    }
}
