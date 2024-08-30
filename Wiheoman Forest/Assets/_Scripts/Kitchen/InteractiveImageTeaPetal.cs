using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTeaPetal : InteractiveImageBase
{
    Vector3 startRectPosition;
    Vector2 mouseToUiOffset;
    GameObject canvas;
    Canvas canvasComponent;
    RectTransform myRectTransform;
    RectTransform canvasRectTransform;

    private void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        
        startRectPosition = myRectTransform.localPosition;


        canvas = CanvasController.instance.GetCanvas();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        canvasComponent = canvas.GetComponent<Canvas>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 resultPosition = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvasComponent.worldCamera, out resultPosition);


        mouseToUiOffset = (Vector2)myRectTransform.position - resultPosition;

    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 resultPosition = new Vector2();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvasComponent.worldCamera,
            out resultPosition);

        myRectTransform.position = resultPosition + mouseToUiOffset;

        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = worldPosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (InteractiveImageTeapotLid.isEntered)
        {
            TeaIngredientController.instance.PutPetal();
        }
        myRectTransform.localPosition = startRectPosition;
    }
}
