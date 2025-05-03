using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///     
/// </summary>
/// <remarks>
///     Make sure call base.Start() in derived class's Start()!
/// </remarks>
public class InteractiveImageDragAndMove : InteractiveImageBase
{
    protected Vector3 startPosition;
    protected Vector2 mouseToUiOffsetVector2;
    protected Vector3 mouseToUiOffsetVector3;
    protected GameObject canvasGameObject;
    protected Canvas canvasComponent;
    protected RectTransform myRectTransform;
    protected RectTransform canvasRectTransform;

    public override void OnPointerDown(PointerEventData eventData)
    {
        Vector3 m_resultPosition = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvasComponent.worldCamera,
            out m_resultPosition);
        mouseToUiOffsetVector3 = transform.position - m_resultPosition;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        transform.position = GetNewFramePosition() + mouseToUiOffsetVector3 * 2;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (IsDragSuccess())
        {
            DoAfterDragSuccess();
        }
        else
        {
            DoAfterDragFailure();
        }
    }

    protected virtual bool IsDragSuccess()
    { 
        return false;
    }

    protected virtual void DoAfterDragSuccess()
    {
        return;
    }

    protected virtual void DoAfterDragFailure()
    {
        transform.position = startPosition;
    }

    protected void BaseStart()
    {
        myRectTransform = GetComponent<RectTransform>();
        startPosition = transform.position;

        canvasGameObject = CanvasController.instance.GetCanvas();
        canvasRectTransform = canvasGameObject.GetComponent<RectTransform>();
        canvasComponent = canvasGameObject.GetComponent<Canvas>();
    }

    protected Vector3 GetMousePosition()
    {
        Vector3 m_resultPosition = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvasComponent.worldCamera,
            out m_resultPosition);
        return m_resultPosition;
    }

    protected Vector3 GetNewFramePosition()
    {
        return GetMousePosition() - mouseToUiOffsetVector3;
    }

    // Start is called before the first frame update
    private void Start()
    {
        BaseStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
