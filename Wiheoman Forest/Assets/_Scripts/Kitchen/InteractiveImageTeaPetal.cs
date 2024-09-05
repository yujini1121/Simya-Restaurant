using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTeaPetal : InteractiveImageDragAndMove
{
    protected override bool IsDragSuccess()
    {
        return InteractiveImageTeapotLid.isEntered;
    }

    protected override void DoAfterDragSuccess()
    {
        TeaIngredientController.instance.PutPetal();
        transform.position = startPosition;
    }

    private void Start()
    {
        BaseStart();
    }
}
