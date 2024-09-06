using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaIngredientController : MonoBehaviour
{
    public static TeaIngredientController instance;

    int petalCount = 0;
    [SerializeField] GameObject petalBasketGameobject;
    [SerializeField] GameObject waterBottleGameobject;
    [SerializeField] GameObject finishButtonGameobject;
    float startTimeOfBoil;

    public void PutPetal()
    {
        petalCount++;

        if (petalCount >= 5)
        {
            petalBasketGameobject.SetActive(false);
            waterBottleGameobject.SetActive(true);
            InteractiveImageTeapot.instance.AddPetal();
        }
    }

    public void PutWater()
    {
        waterBottleGameobject.SetActive(false);
        finishButtonGameobject.SetActive(true);

        startTimeOfBoil = Time.time;
    }

    public void Finish()
    {
        float term = Time.time - startTimeOfBoil;

        if (term < 3.0f)
        {
            Debug.Log($"{term} -> Bad");
        }
        else if (term < 5.0f)
        {
            Debug.Log($"{term} -> Moderate");
        }
        else if (term < 7.0f)
        {
            Debug.Log($"{term} -> Good");
        }
        else
        {
            Debug.Log($"{term} -> Bad");
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        waterBottleGameobject.SetActive(false);
        finishButtonGameobject.SetActive(false);
    }
}
