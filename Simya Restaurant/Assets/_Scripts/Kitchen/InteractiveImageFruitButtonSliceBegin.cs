using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageFruitButtonSliceBegin : InteractiveImageDragAndMove
{
    [SerializeField] GameObject knifeGameObject;
    Vector3 buttonToKnifeOffset;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (FruitGatheringController.instance.isKnifeHolding)
        {
            FruitGatheringController.instance.BeginSlice();
            buttonToKnifeOffset = knifeGameObject.transform.position - transform.position;
        }
        Cursor.visible = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (FruitGatheringController.instance.isKnifeSlicing)
        {
            transform.position = new Vector3(transform.position.x, GetNewFramePosition().y, transform.position.z);
            knifeGameObject.transform.position = transform.position + buttonToKnifeOffset;
        }
    }

    protected override bool IsDragSuccess()
    {
        Cursor.visible = true;

        if (FruitGatheringController.instance.isKnifeSlicing)
        {
            return FruitGatheringController.instance.IsEndSlice();
        }

        return false;
    }

    protected override void DoAfterDragSuccess()
    {
        FruitGatheringController.instance.EndSlice();
    }

    protected override void DoAfterDragFailure()
    {

    }
}
