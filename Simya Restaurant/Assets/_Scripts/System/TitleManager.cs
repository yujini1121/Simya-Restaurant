using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;

    [Header("Option")]
    [SerializeField] private GameObject OptionUI;

    [Header("Credit")]
    [SerializeField] private GameObject creditUI;
    [SerializeField] private RectTransform creditText;
    [SerializeField] private float scrollSpeed = 70f;

    private bool isScrolling = false;
    private float stopPosY = 0;

    void Start()
    {
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        creditsButton.onClick.AddListener(OnCreditsButtonClicked);
    }

    private void Update()
    {
        if(isScrolling)
        {
            if(creditText.anchoredPosition.y < stopPosY)
            {
                creditText.anchoredPosition += new Vector2(0f, scrollSpeed);
            }
            else
            {
                creditText.anchoredPosition = new Vector2(creditText.anchoredPosition.x, stopPosY);
                isScrolling = false;  // 스크롤 종료
            }
        }
    }

    private void OnNewGameButtonClicked()
    {

    }

    private void OnLoadGameButtonClicked()
    {

    }

    private void OnSettingsButtonClicked()
    {
        OptionUI.SetActive(true);
    }

    private void OnCreditsButtonClicked()
    {
        creditText.anchoredPosition = new Vector2(creditText.anchoredPosition.x, -700f);

        creditUI.SetActive(true);

        isScrolling = true;
    }
}
