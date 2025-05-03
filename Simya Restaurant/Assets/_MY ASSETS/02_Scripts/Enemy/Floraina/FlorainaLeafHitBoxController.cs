using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlorainaLeafHitBoxController : MonoBehaviour
{
    public float timeForMoving
    {
        get => timeForMovingInspector;
    }
    public float timeForWaiting
    {
        get => timeForWaitingInspector;
    }

    [SerializeField] private float timeForWaitingInspector = 2.0f;
    [SerializeField] private float timeForMovingInspector = 2.0f;
    [SerializeField] private float lengthForMoving;
    private float stackedTime;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool isStarted = false;
    private MeshRenderer myMeshRenderer;

    private void Awake()
    {
        stackedTime = 0.0f;
        startPosition = transform.position;
        endPosition = transform.position + new Vector3(-lengthForMoving, 0, 0);
    }

    private void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        myMeshRenderer.enabled = false;
        StartCoroutine(
            UtilityFunctions.RunAfterDelay(
                timeForWaitingInspector,
                () =>
                {
                    myMeshRenderer.enabled = true;
                    GetComponent<Collider>().enabled = true;
                    isStarted = true;
                    Destroy(gameObject, timeForMoving);
                }
                ));
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            stackedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, endPosition, stackedTime / timeForMoving);
        }
    }
}
