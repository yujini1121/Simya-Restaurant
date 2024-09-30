using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageJamJudge : InteractiveImageBase
{
    // Start is called before the first frame update
    void Start()
    {
        SlimeJamSpreadingController.instance.Add();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && InteractiveImageJamBottle.instance.isJamPicked)
        {
            Placed();
        }
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (InteractiveImageJamBottle.instance.isJamPicked)
        {
            Placed();
        }
    }

    void Placed()
    {
        SlimeJamSpreadingController.instance.Placed();
        Destroy(gameObject);
    }
}
