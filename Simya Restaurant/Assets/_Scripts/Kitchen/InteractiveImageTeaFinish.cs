using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTeaFinish : InteractiveImageBase
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        TeaIngredientController.instance.Finish();
    }
}
