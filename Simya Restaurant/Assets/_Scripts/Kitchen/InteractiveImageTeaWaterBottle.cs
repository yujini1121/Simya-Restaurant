using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTeaWaterBottle : InteractiveImageDragAndMove
{
    protected override bool IsDragSuccess()
    {
        return InteractiveImageTeapotLid.isEntered;
    }

    protected override void DoAfterDragSuccess()
    {
        TeaIngredientController.instance.PutWater();
        transform.position = startPosition;
    }

    private void Start()
    {
        BaseStart();
    }
}
