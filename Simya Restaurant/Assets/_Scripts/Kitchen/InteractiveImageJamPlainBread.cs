using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageJamPlainBread : InteractiveImageBase
{
    public static InteractiveImageJamPlainBread instance;
    public bool isOnBread = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        isOnBread = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        isOnBread = false;
    }
}
