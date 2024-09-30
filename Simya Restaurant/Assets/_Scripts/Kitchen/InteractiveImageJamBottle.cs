using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveImageJamBottle : InteractiveImageDragAndMove
{
    public static InteractiveImageJamBottle instance;
    public bool isJamPicked = false;
    [SerializeField] private GameObject jamBottleGameObject;
    [SerializeField] private GameObject jamBlobGameObject;
    [SerializeField] private GameObject jamTraceGameObject;
    [SerializeField] private GameObject jamTraceTransformParentGameObject;
    [SerializeField] private Sprite imageFullJam;
    [SerializeField] private Sprite imageHalfJam;
    [SerializeField] private float jamDragTerm;
    [SerializeField] private float jamTraceSize;
    private UnityEngine.UI.Image jamBottleImageComponent;
    private Vector3 lastDragPosition;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        jamBottleImageComponent = jamBottleGameObject.GetComponent<UnityEngine.UI.Image>();

        BaseStart();
        myRectTransform = jamBlobGameObject.GetComponent<RectTransform>();
        lastDragPosition = transform.position;
        jamDragTerm *= jamDragTerm;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        isJamPicked = true;
        jamBottleImageComponent.sprite = imageHalfJam;
        jamBlobGameObject.SetActive(true);

        Vector3 m_resultPosition = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvasComponent.worldCamera,
            out m_resultPosition);
        mouseToUiOffsetVector3 = transform.position - m_resultPosition;
        jamBlobGameObject.transform.position = GetNewFramePosition();

        Cursor.visible = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        jamBlobGameObject.transform.position = GetNewFramePosition() + mouseToUiOffsetVector3;

        if ((jamBlobGameObject.transform.position - lastDragPosition).sqrMagnitude > jamDragTerm &&
            InteractiveImageJamPlainBread.instance.isOnBread)
        {
            lastDragPosition = jamBlobGameObject.transform.position;
            GameObject one = Instantiate(jamTraceGameObject, jamBlobGameObject.transform.position, jamTraceGameObject.transform.rotation);
            one.transform.SetParent(jamTraceTransformParentGameObject.transform);
            one.transform.localScale = new Vector3(jamTraceSize, jamTraceSize);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Cursor.visible = true;

        isJamPicked = false;
        jamBottleImageComponent.sprite = imageFullJam;
        jamBlobGameObject.SetActive(false);
    }
}
