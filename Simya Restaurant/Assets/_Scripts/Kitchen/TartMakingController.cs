using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartMakingController : MonoBehaviour
{
    public static TartMakingController instance;

    [SerializeField] GameObject emptyTartGameObject;
    [SerializeField] GameObject filledTartGameObject;
    [SerializeField] GameObject fillingStartGameObject;
    [SerializeField] GameObject fillingClickAreaGameObject;
    [SerializeField] GameObject[] fruitColliderGameObject;
    [SerializeField] GameObject fillingCreamGameObject;
    [SerializeField] GameObject tartFruitGameObject;
    [SerializeField] GameObject finishGameObject;
    [SerializeField] GameObject fillingHandleGameObject;
    InteractiveImageTartFruitCollider collider = null;
    [SerializeField] float fillingTime;
    float filledTime = 0.0f;
    float filledStartTime;
    float creamSize = 0.7f;
    int fruitCount = 0;
    Coroutine coroutine;

    public void AddFruit()
    {
        fruitCount++;
    }
    public void ReadyFilling()
    {
        //fillingStartGameObject.SetActive(false);
        fillingClickAreaGameObject.SetActive(true);
    }
    public void StartFilling()
    {
        filledStartTime = Time.time - filledTime;
        fillingCreamGameObject.SetActive(true);
        coroutine = StartCoroutine(FillingCreamCoroutine());
    }
    public void PauseFilling()
    {
        StopCoroutine(coroutine);
    }

    public void EndFillingAndNext()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        fillingClickAreaGameObject.SetActive(false);
        emptyTartGameObject.SetActive(false);
        filledTartGameObject.SetActive(true);
        tartFruitGameObject.SetActive(true);
        finishGameObject.SetActive(true);
        foreach (GameObject go in fruitColliderGameObject)
        {
            go.SetActive(true);
        }
    }

    private void Awake()
    {
        if (fillingTime <= 0.0f)
        {
            fillingTime = 1.0f;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fillingClickAreaGameObject.SetActive(false);
        tartFruitGameObject.SetActive(false);
        finishGameObject.SetActive(false);
        foreach (GameObject go in fruitColliderGameObject)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public EFoodRank GetRank()
    {
        int fillingResult = 0;
        if (filledTime / fillingTime <= 0.5f)
        {
            fillingResult = 0;
        }
        else if (filledTime / fillingTime < 0.9f)
        {
            fillingResult = 1;
        }
        else if (filledTime / fillingTime <= 1.0f)
        {
            fillingResult = 2;
        }
        else if (filledTime / fillingTime <= 1.2f)
        {
            fillingResult = 1;
        }

        int fruitResult = 0;
        if (fruitCount <= 1)
        {
            fruitResult = 0;
        }
        else if (fruitCount <= 2)
        {
            fruitResult = 1;
        }
        else
        {
            fruitResult = 2;
        }

        switch (Math.Min(fillingResult, fruitResult))
        {
            case 0:
                return EFoodRank.Bad;
            case 1:
                return EFoodRank.Standard;
            case 2:
                return EFoodRank.Good;
            default:
                return EFoodRank.None;
        }
    }

    IEnumerator FillingCreamCoroutine()
    {
        while (true)
        {
            filledTime = Time.time - filledStartTime;

            Debug.Log(filledTime);
            if (filledTime > fillingTime * 2) filledTime = fillingTime;

            fillingCreamGameObject.transform.localScale =
                Vector3.Lerp(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(creamSize, creamSize, 1.0f), filledTime / fillingTime);
            yield return null;
        }


    }
}
