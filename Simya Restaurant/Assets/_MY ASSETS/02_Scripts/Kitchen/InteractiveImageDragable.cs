using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Obsolete]
public class InteractiveImageDragable : InteractiveImageBase
{
    //public override void OnBeginDrag(PointerEventData eventData)
    //{
    //    Vector2 resultPosition = new Vector2();
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        canvasRectTransform,
    //        Input.mousePosition,
    //        canvasComponent.worldCamera, out resultPosition);
    //    mouseToUiOffset = (Vector2)myRectTransform.position - resultPosition;
    //}

    //public override void OnDrag(PointerEventData eventData)
    //{
    //    Vector2 resultPosition = new Vector2();
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        canvasRectTransform,
    //        Input.mousePosition,
    //        canvasComponent.worldCamera,
    //        out resultPosition);
    //    myRectTransform.position = resultPosition + mouseToUiOffset;
    //    //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    //transform.position = worldPosition;
    //}

    //public override void OnEndDrag(PointerEventData eventData)
    //{
    //    if (InteractiveImagePetalBasket.isEntered)
    //    {
    //        Debug.Log("꽃잎을 담았습니다.");
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        myRectTransform.localPosition = startRectPosition;
    //        Debug.Log("바구니 밖입니다.");
    //    }
    //}

    //// Start is called before the first frame update
    //void Start()
    //{
    //    myRectTransform = GetComponent<RectTransform>();
    //    startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    //    startRectPosition = myRectTransform.localPosition;
    //    canvas = CanvasController.instance.GetCanvas();
    //    canvasRectTransform = canvas.GetComponent<RectTransform>();
    //    canvasComponent = canvas.GetComponent<Canvas>();
    //}
}
