using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TartMakingController : MonoBehaviour
{
    public static TartMakingController instance;

    [SerializeField] GameObject emptyTartGameObject;
    [SerializeField] GameObject filledTartGameObject;
    [SerializeField] GameObject fillingStartGameObject;
    [SerializeField] GameObject fillingGameObject;
    [SerializeField] GameObject fillingClickAreaGameObject;
    [SerializeField] GameObject[] fruitColliderGameObject;
    [SerializeField] GameObject fillingCreamGameObject;
    InteractiveImageTartFruitCollider collider = null;
    [SerializeField] float fillingTime;
    float filledTime;
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
        fillingStartGameObject.SetActive(false);
        fillingGameObject.SetActive(true);
        fillingClickAreaGameObject.SetActive(true);
    }
    public void StartFilling()
    {
        filledTime = 0.0f;
        filledStartTime = Time.time;
        fillingCreamGameObject.SetActive(true);
        coroutine = StartCoroutine(FillingCreamCoroutine());
    }
    public void EndFillingAndNext()
    {
        StopCoroutine(coroutine);
        fillingGameObject.SetActive(false);
        fillingClickAreaGameObject.SetActive(false);
        emptyTartGameObject.SetActive(false);
        filledTartGameObject.SetActive(true);
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
        fillingGameObject.SetActive(false);
        foreach (GameObject go in fruitColliderGameObject)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    EFoodRank GetRank()
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

        if (fillingResult == 0 & fruitCount < 2)
        {
            return EFoodRank.Bad;
        }
        else if (fillingResult == 2 && fruitCount == 3)
        {
            return EFoodRank.Good;
        }


        return EFoodRank.Standard;
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
