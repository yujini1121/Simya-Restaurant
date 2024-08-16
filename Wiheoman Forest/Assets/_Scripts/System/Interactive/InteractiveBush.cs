using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBush : InteractiveObjectBase
{
    [SerializeField] GameObject droppingItemMesh;
    [SerializeField] GameObject dropItemPrefab;
    [SerializeField] float droppingTime;
    [SerializeField] RangedRectangle dropArea;
    float curveY = 5.0f;
    bool isGatherable = true;

    private void Awake()
    {
        if (droppingTime <= 0.0f)
        {
            droppingTime = 1.0f;
        }
    }

    public override void DoInteractiveWithThis()
    {
        if (isGatherable == false)
        {
            return;
        }

        isGatherable = false;

        GameObject instantiatedItemMesh = Instantiate(droppingItemMesh, transform.position, droppingItemMesh.transform.rotation);
        Vector3 endPoint = transform.position + dropArea.GetRandomPoint();
        StartCoroutine(
            UtilityFunctions.MoveOnBezierCurve(
                transform.position,
                endPoint,
                UtilityFunctions.GetMiddleForBezierCurve(transform.position, endPoint, curveY),
                instantiatedItemMesh,
                droppingTime
                )
            );
        StartCoroutine(
            UtilityFunctions.RunAfterDelay(
                droppingTime,
                () =>
                {
                    Instantiate(dropItemPrefab, endPoint, dropItemPrefab.transform.rotation);
                    Destroy(instantiatedItemMesh);
                }
                ));
        
    }
}
