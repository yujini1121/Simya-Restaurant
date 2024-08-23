using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaIngredientController : MonoBehaviour
{
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
            Debug.Log("Bad");
        }
        else if (term < 5.0f)
        {
            Debug.Log("Moderate");
        }
        else
        {
            Debug.Log("Good");
        }
    }
}
