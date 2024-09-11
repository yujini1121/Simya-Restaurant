using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageFruitButtonSliceBegin : InteractiveImageBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (FruitGatheringController.instance.isKnifeHolding)
        {
            FruitGatheringController.instance.BeginSlice();
            FruitGatheringController.instance.buttonToKnifeOffset =
                FruitGatheringController.instance.GetKnifePosiiton() - transform.position;
        }
    }
    

}
