using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTartFillingStart : InteractiveImageDragAndMove
{
    [SerializeField] GameObject handlePosition;
    public static bool isHoldingFiller = false;

    
    public override void OnPointerDown(PointerEventData eventData)
    {
        Vector3 m_resultPosition = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvasComponent.worldCamera,
            out m_resultPosition);
        mouseToUiOffsetVector3 = transform.position - handlePosition.transform.position;
        
        TartMakingController.instance.ReadyFilling();
        Cursor.visible = false;
        isHoldingFiller = true;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector3 m_resultPosition = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvasComponent.worldCamera,
            out m_resultPosition);
        transform.position = m_resultPosition + mouseToUiOffsetVector3;
    }

    protected override void DoAfterDragFailure()
    {
        Cursor.visible = true;
        base.DoAfterDragFailure();
    }

    private void Update()
    {
        if (isHoldingFiller && Input.GetMouseButtonUp(0))
        {
            TartMakingController.instance.EndFillingAndNext();
            isHoldingFiller = false;
            gameObject.SetActive(false);
        }
    }
}
