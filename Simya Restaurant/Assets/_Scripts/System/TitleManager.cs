using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    [Header("Option")]
    [SerializeField] private GameObject OptionUI;

    [Header("Credit")]
    [SerializeField] private GameObject creditUI;
    [SerializeField] private RectTransform creditText;
    [SerializeField] private float scrollSpeed = 70f;

    private int currentIndex = 0;
    private bool isScrolling = false;
    private float stopPosY = 0;

    void Start()
    {
        ButtonEffect(currentIndex, true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-1);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(1);
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            buttons[currentIndex].onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseActiveUI();
        }
    }

    private void MoveSelection(int direction)
    {
        ButtonEffect(currentIndex, false);

        currentIndex = (currentIndex + direction + buttons.Length) % buttons.Length;

        ButtonEffect(currentIndex, true);
    }

    private void ButtonEffect(int index, bool isSelected)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);

        TextMeshProUGUI text = buttons[index].GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;
        }
    }

    public void OnNewGameButtonClicked()
    {
        DataController.instance.MakeNew();
        DataController.instance.SaveData();

        SceneTransition.Instance.ChangeScene("Forest"); // Home¿∏∑Œ ¥ŸΩ√ ºˆ¡§
    }

    public void OnLoadGameButtonClicked()
    {
        DataController.instance.LoadData();
        Debug.Log("±‚¡∏ µ•¿Ã≈Õ ∑ŒµÂ");

        SceneTransition.Instance.ChangeScene("Home");
    }

    public void OnSettingsButtonClicked()
    {
        OptionUI.SetActive(true);
    }

    public void OnCreditsButtonClicked()
    {
        creditText.anchoredPosition = new Vector2(creditText.anchoredPosition.x, -700f);
        creditUI.SetActive(true);

        if (!isScrolling)
        {
            StartCoroutine(ScrollCredits());
        }
    }

    private IEnumerator ScrollCredits()
    {
        isScrolling = true;

        while (creditText.anchoredPosition.y < stopPosY)
        {
            creditText.anchoredPosition += new Vector2(0f, scrollSpeed);
            yield return null;
        }

        creditText.anchoredPosition = new Vector2(creditText.anchoredPosition.x, stopPosY);
        isScrolling = false;
    }

    private void CloseActiveUI()
    {
        if (OptionUI.activeSelf)
        {
            OptionUI.SetActive(false);
            Debug.Log("Option UI ¥›»˚");
        }

        if (creditUI.activeSelf)
        {
            creditUI.SetActive(false);
            isScrolling = false;
            Debug.Log("Credit UI ¥›»˚");
        }
    }
}
