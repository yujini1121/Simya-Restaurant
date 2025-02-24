using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanghuluMaking : MonoBehaviour
{
    [SerializeField] float targetValue; // 65;
    [SerializeField] float goodRange; // 5
    [SerializeField] float normalRange; // 50 ~ 80 -> 15
    [SerializeField] float dippingTime;
    //[SerializeField] Sprite nonDipSprite;
    //[SerializeField] Sprite dippingSprite;
    [SerializeField] GameObject dippingGameObject;
    [SerializeField] GameObject dippingGameObjectFinish;
    [SerializeField] UnityEngine.UI.Image image;
    [SerializeField] UnityEngine.UI.Image fillBar;
    float startTime = 0;
    float m_result = 0;
    bool isDipped = false;
    bool isFinished = false;
    UnityEngine.UI.Image dippingImage;
    Vector3 positionStart;
    Vector3 positionEnd;
    FoodComponent resultDish;

    public void PressDipButton()
    {
        if (isFinished) return;
        isDipped = true;
        startTime = Time.time;

        //image.sprite = dippingSprite;
    }

    public void ReleaseButton()
    {
        if (isFinished) return;
        isFinished = true;
        isDipped = false;

        //image.sprite = nonDipSprite;

        if (m_result > targetValue - goodRange && m_result < targetValue + goodRange) resultDish.rank = EFoodRank.Good;
        else if (m_result > targetValue - normalRange && m_result < targetValue + normalRange) resultDish.rank = EFoodRank.Standard;
        else resultDish.rank = EFoodRank.Bad;

        Debug.Log($"DEBUG_TanghuluMaking.ReleaseButton() : °á°ú = {resultDish.rank}");
    }

    // Start is called before the first frame update
    void Start()
    {
        resultDish = new FoodComponent("tanghulu", 10);
        fillBar.fillAmount = 0.0f;

        dippingImage = dippingGameObject.GetComponent<UnityEngine.UI.Image>();
        positionStart = dippingGameObject.transform.position;
        positionEnd = dippingGameObjectFinish.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDipped)
        {
            m_result = m_GetValue();
            fillBar.fillAmount = m_result;

            dippingImage.fillAmount = 1 - m_result;
            dippingGameObject.transform.position = Vector3.Lerp(positionStart, positionEnd, m_result);
        }
    }

    private float m_GetValue()
    {
        return Mathf.Max((Time.time - startTime) / dippingTime, 0);
    }
}
