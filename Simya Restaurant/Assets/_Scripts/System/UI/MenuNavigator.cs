using UnityEngine;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    /* 
     * 이 스크립트는 
     * 위 아래 방향키로 고르고, 엔터로 선택할 수 있는
     * 모든 메뉴 UI에 대한 스크립트입니다.
     * 
     * 메뉴 UI에 이 스크립트를 부착한 후,
     * Button 배열에 탐색할 버튼 전부를 할당하고,
     * 각 Button에 OnClick 메서드로 무슨 메서드를 호출할지만 추가해주면 됩니다.
     * 
    */


    public Button[] menuButtons;
    private int currentSelection = 0;
    private Outline currentOutline;


    void Start()
    {
        UpdateMenuSelection();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            NavigateUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            NavigateDown();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SelectMenu();
        }
    }


    void NavigateUp()
    {
        currentSelection--;
        if (currentSelection < 0)
            currentSelection = menuButtons.Length - 1;

        UpdateMenuSelection();
    }

    void NavigateDown()
    {
        currentSelection++;
        if (currentSelection >= menuButtons.Length)
            currentSelection = 0;

        UpdateMenuSelection();
    }

    void SelectMenu()
    {
        menuButtons[currentSelection].onClick.Invoke();
    }

    void UpdateMenuSelection()
    {
        // 이전에 선택된 버튼에 아웃라인 제거
        if (currentOutline != null)
        {
            Destroy(currentOutline);
        }

        // 새로 선택된 버튼에 아웃라인 추가
        Button selectedButton = menuButtons[currentSelection];
        currentOutline = selectedButton.gameObject.AddComponent<Outline>();
        currentOutline.effectColor = Color.green; // 아웃라인 색상
        currentOutline.effectDistance = new Vector2(5, 5); // 아웃라인 두께
    }

    public void TestButton1()
    {
        Debug.Log("Test Button 1");
    }

    public void TestButton2()
    {
        Debug.Log("Test Button 2");
    }
}
