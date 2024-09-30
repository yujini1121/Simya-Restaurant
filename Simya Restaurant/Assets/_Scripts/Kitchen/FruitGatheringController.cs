using Unity.VisualScripting;
using UnityEngine;

public class FruitGatheringController : MonoBehaviour
{
    static public FruitGatheringController instance;
    public bool isKnifeHolding = false;
    public bool isKnifeSlicing = false;

    [SerializeField] GameObject fruitGameObjectSide;
    [SerializeField] GameObject fruitGameObjectTop;
    [SerializeField] GameObject fruitGameObjectPeeled;
    [SerializeField] GameObject knifeGameObject;
    [SerializeField] GameObject middleGameObject;
    [SerializeField] GameObject positionKnifeHandleGameObject;
    [SerializeField] GameObject cuttingEndGameObject;
    [SerializeField] GameObject buttonSliceStartGameObject;
    [SerializeField] GameObject spinAreaGameObject;

    //[SerializeField] GameObject buttonKnifeReadyGameObject;
    [SerializeField] GameObject guideSliceGameObject;
    private Vector3 knifeStartPosition;
    private Vector2 mouseToUiOffsetVector2;
    private Vector3 mouseToUiOffsetVector3;
    private GameObject canvasGameObject;
    private Canvas canvasComponent;
    private RectTransform knifeRectTransform;
    private RectTransform canvasRectTransform;
    private bool isSliceDone = false;


    public void PickKnife()
    {
        isKnifeHolding = true;
        //buttonKnifeReadyGameObject.SetActive(false);

        Vector3 m_resultPosition = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvasRectTransform,
            Input.mousePosition,
            canvasComponent.worldCamera,
            out m_resultPosition);
        mouseToUiOffsetVector3 = knifeGameObject.transform.position - m_resultPosition + m_resultPosition - positionKnifeHandleGameObject.transform.position;
    }

    public void BeginSlice()
    {
        if (isKnifeHolding)
        {
            isKnifeHolding = false;
            isKnifeSlicing = true;

            guideSliceGameObject.SetActive(false);
        }

    }
    public bool IsEndSlice()
    {
        return knifeGameObject.transform.position.y < cuttingEndGameObject.transform.position.y;
    }

    public void EndSlice()
    {
        fruitGameObjectSide.SetActive(false);
        fruitGameObjectTop.SetActive(true);

        knifeGameObject.SetActive(false);
        cuttingEndGameObject.SetActive(false);
        buttonSliceStartGameObject.SetActive(false);

        spinAreaGameObject.SetActive(true);

        isSliceDone = true;
    }

    public Vector3 GetKnifePosiiton()
    {
        return knifeGameObject.transform.position;
    }


    public void EndGetFruit()
    {
        Debug.Log("MEOW!");

        fruitGameObjectTop.SetActive(false);
        spinAreaGameObject.SetActive(false);
        fruitGameObjectPeeled.SetActive(true);
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fruitGameObjectTop.SetActive(false);
        fruitGameObjectPeeled.SetActive(false);
        spinAreaGameObject.SetActive(false);

        knifeRectTransform = knifeGameObject.GetComponent<RectTransform>();
        knifeStartPosition = knifeGameObject.transform.position;

        canvasGameObject = CanvasController.instance.GetCanvas();
        canvasRectTransform = canvasGameObject.GetComponent<RectTransform>();
        canvasComponent = canvasGameObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isKnifeHolding)
        {
            Vector3 m_resultPosition = new Vector3();
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvasRectTransform,
                Input.mousePosition,
                canvasComponent.worldCamera,
                out m_resultPosition);
            knifeGameObject.transform.position = m_resultPosition + mouseToUiOffsetVector3;
        }
        //if (isKnifeSlicing && (isSliceDone == false))
        //{
        //    if (knifeGameObject.transform.position.y < cuttingEndGameObject.transform.position.y)
        //    {
        //        // ½ä±â ¿Ï·á
        //        fruitGameObjectSide.SetActive(false);
        //        fruitGameObjectTop.SetActive(true);

        //        knifeGameObject.SetActive(false);
        //        cuttingEndGameObject.SetActive(false);
        //        buttonSliceStartGameObject.SetActive(false);

        //        spinAreaGameObject.SetActive(true);

        //        isSliceDone = true;
        //    }
        //}
    }
}
