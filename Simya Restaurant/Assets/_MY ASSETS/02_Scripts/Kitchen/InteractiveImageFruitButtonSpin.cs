using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageFruitButtonSpin : InteractiveImageDragAndMove
{
    public Vector2 prevFrameVector = Vector3.zero;
    public float angleSumDegree = 0.0f;

    public override void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosVector3 = GetMousePosition();
        Vector2 mousePosVector2 = new Vector2(mousePosVector3.x, mousePosVector3.y);

        Vector2 currentVector = mousePosVector2 - new Vector2(transform.position.x, transform.position.y);

        if (prevFrameVector != Vector2.zero)
        {
            angleSumDegree += Vector2.SignedAngle(prevFrameVector, currentVector);
        }

        prevFrameVector = currentVector;

    }

    protected override bool IsDragSuccess()
    {
        return Mathf.Abs(angleSumDegree) >= 360.0f;
    }

    protected override void DoAfterDragFailure()
    {
        angleSumDegree = 0.0f;
        prevFrameVector = Vector3.zero;
    }

    protected override void DoAfterDragSuccess()
    {
        FruitGatheringController.instance.EndGetFruit();
        gameObject.SetActive(false);
    }
}
