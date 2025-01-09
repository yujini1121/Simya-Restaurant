using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


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

    private Button lastSelectedButton;

    private bool isNavigating = false;
    private float navigationDelay = 0.2f;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);
        SelectedButtonEffect(newGameButton);
        lastSelectedButton = newGameButton;
    }

    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (isNavigating) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(NavigateWithDelay("Previous", current));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(NavigateWithDelay("Next", current));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCurrentButton(current);
        }

        UpdateSelectedButtonEffect(current);
    }

    private IEnumerator NavigateWithDelay(string direction, GameObject currentButton)
    {
        isNavigating = true;

        NavigateButton(direction, currentButton);

        yield return new WaitForSeconds(navigationDelay);

        isNavigating = false;
    }

    private void NavigateButton(string choice, GameObject currentButton)
    {
        if (currentButton == null)
        {
            return;
        }

        Selectable selectable = null;
        if (choice == "Previous")
        {
            selectable = currentButton.GetComponent<Selectable>().FindSelectableOnUp();
        }
        else if(choice == "Next")
        {
            selectable = currentButton.GetComponent<Selectable>().FindSelectableOnDown();
        }

        
        if (selectable != null)
        {
            EventSystem.current.SetSelectedGameObject(selectable.gameObject);
        }
    }

    private void ExecuteCurrentButton(GameObject currentButton)
    {
        if (currentButton == null) return;

        Button button = currentButton.GetComponent<Button>();
        if(button != null)
        {
            button.onClick.Invoke();
        }
    }

    public void OnNewGameButtonClicked()
    {
        // 새 데이터 저장 함수 호출
        // 새 데이터 초기화
        // 초기화 데이터 저장
        // 씬 전환
    }

    public void OnLoadGameButtonClicked()
    {
        DataController.instance.LoadData();
        Debug.Log("기존 데이터 로드");

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

        if(!isScrolling)
        {
            StartCoroutine(ScrollCredits());
        }
    }

    private IEnumerator ScrollCredits()
    {
        isScrolling = true;

        while(creditText.anchoredPosition.y < stopPosY)
        {
            creditText.anchoredPosition += new Vector2(0f, scrollSpeed);
            yield return null;
        }

        creditText.anchoredPosition = new Vector2(creditText.anchoredPosition.x, stopPosY);
        isScrolling = false;
    }

    private void ResetButtonEffect(Button button)
    {
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.fontStyle = FontStyles.Normal;
        }
    }

    private void SelectedButtonEffect(Button button)
    {
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.fontStyle = FontStyles.Bold;
        }
    }

    private void UpdateSelectedButtonEffect(GameObject currentButton)
    {
        if (currentButton == null) return;

        Button selectedButton = currentButton.GetComponent<Button>();
        if (selectedButton != null && selectedButton != lastSelectedButton)
        {
            // 이전 버튼 효과 초기화
            ResetButtonEffect(lastSelectedButton);

            // 새 버튼 효과 적용
            SelectedButtonEffect(selectedButton);

            // 선택된 버튼 갱신
            lastSelectedButton = selectedButton;
        }
    }
}
