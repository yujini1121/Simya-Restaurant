using UnityEngine;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    /* 
     * �� ��ũ��Ʈ�� 
     * �� �Ʒ� ����Ű�� ����, ���ͷ� ������ �� �ִ�
     * ��� �޴� UI�� ���� ��ũ��Ʈ�Դϴ�.
     * 
     * �޴� UI�� �� ��ũ��Ʈ�� ������ ��,
     * Button �迭�� Ž���� ��ư ���θ� �Ҵ��ϰ�,
     * �� Button�� OnClick �޼���� ���� �޼��带 ȣ�������� �߰����ָ� �˴ϴ�.
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
        // ������ ���õ� ��ư�� �ƿ����� ����
        if (currentOutline != null)
        {
            Destroy(currentOutline);
        }

        // ���� ���õ� ��ư�� �ƿ����� �߰�
        Button selectedButton = menuButtons[currentSelection];
        currentOutline = selectedButton.gameObject.AddComponent<Outline>();
        currentOutline.effectColor = Color.green; // �ƿ����� ����
        currentOutline.effectDistance = new Vector2(5, 5); // �ƿ����� �β�
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
