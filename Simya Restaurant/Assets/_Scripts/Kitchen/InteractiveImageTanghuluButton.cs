using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTanghuluButton : InteractiveImageBase
{
    [SerializeField] TanghuluMaking tanghuluScript;

    public override void OnPointerDown(PointerEventData eventData)
    {
        tanghuluScript.PressDipButton();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        tanghuluScript.ReleaseButton();
    }
}
