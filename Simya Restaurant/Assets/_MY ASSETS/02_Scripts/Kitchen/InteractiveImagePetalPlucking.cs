using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InteractiveImagePetalPlucking : InteractiveImageDragAndMove
{
    protected override bool IsDragSuccess()
    {
        return InteractiveImagePetalBasket.isEntered;
    }

    protected override void DoAfterDragSuccess()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        BaseStart();
    }
}
