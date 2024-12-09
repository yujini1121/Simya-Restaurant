using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroll : MonoBehaviour
{
    [SerializeField] private RectTransform creditText;
    [SerializeField] private float scrollSpeed = 1f;

    private float stopPosY = -200f;
    private bool isScrolling = false;

    private void Update()
    {
        if (isScrolling)
        {
            creditText.anchoredPosition += new Vector2(0f, scrollSpeed * Time.deltaTime);

            if (creditText.anchoredPosition.y >= stopPosY)
            {
                isScrolling = false;
            }
        }
    }

    public void StartScroll()
    {
        creditText.anchoredPosition = new Vector2(creditText.anchoredPosition.x, -740f);

        isScrolling = true;
    }
}
