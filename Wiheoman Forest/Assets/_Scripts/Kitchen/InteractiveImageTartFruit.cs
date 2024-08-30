using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveImageTartFruit : InteractiveImageBase
{
    static public InteractiveImageTartFruit instance;

    public InteractiveImageTartFruitCollider targetCollider;

    Vector3 startPosition;
    Vector3 startRectPosition;
    Vector2 mouseToUiOffset;
    GameObject canvas;
    Canvas canvasComponent;
    RectTransform myRectTransform;
    RectTransform canvasRectTransform;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();

        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
        //myRectTransform.position = resultPosition;

        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = worldPosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (targetCollider != null)
        {
            targetCollider.PlaceFruit();
        }
        transform.position = startPosition;
    }
}
