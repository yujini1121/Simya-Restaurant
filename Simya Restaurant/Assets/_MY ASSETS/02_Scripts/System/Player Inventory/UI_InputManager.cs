using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitableUI
{
    public void Init();
    public void Close();
}


[System.Serializable]
public class UI_Element
{
    public static UI_Element instance;

    public string identificationName;
    public GameObject uiObject;
    public bool isActive;
    public KeyCode toggleKey;
    // =========================================================
    // Ư�� UI�� �ܼ��� Active ���¸� �ٲٴ°͸����� ������ �� �ֽ��ϴ�.
    // UI�� ���ų� ������, �߰������� �ʿ��� �Լ� �۾��� �ִٸ�
    // �ش� UI�� ��Ʈ���ϴ� ��ũ��Ʈ���� Start �޼��忡 ������ ��,
    // �װ����� AddInitializeFunction�̳� AddCloseFunction�� ȣ���Ͻñ� �ٶ��ϴ�.
    // ���� �ش� �Լ��� ȣ���� ��, UI_InputManager�� �ν����� â����
    // identificationName�� AddInitializeFunction Ȥ�� AddCloseFunction�� String ����
    // ��ġ���� �׻� Ȯ���Ͻñ� �ٶ��ϴ�.
    // =========================================================
    public bool hasInitializationFunction = false;
    public System.Action InitializeUI = () => { };
    public System.Action CloseUI = () => { };

    public UI_Element() { instance = this; }
}

public class UI_InputManager : MonoBehaviour
{
    [SerializeField] private UI_Element[] UI_Elements;
    [SerializeField] private bool isDebugGameObjectNotExist;

    public void AddInitializeFunction(string uiName, System.Action function)
    {
        int m_foundIndex = M_FindIndex(uiName);

        if (m_foundIndex == -1)
        {
            Debug.LogWarning($"����:UI_InputManager.AddInitializeFunction(uiName = {uiName}, function) : ã������ �̸��� �������� �ʴ� UI �����Դϴ�. �ٽ� Ȯ�� �ٶ��ϴ�.\n���ӿ�����Ʈ�� : {gameObject.name}");
            return;
        }

        UI_Elements[m_foundIndex].InitializeUI += function;
        UI_Elements[m_foundIndex].hasInitializationFunction = true;
    }

    public void AddCloseFunction(string uiName, System.Action function)
    {
        int m_foundIndex = M_FindIndex(uiName);

        if (m_foundIndex == -1)
        {
            Debug.LogWarning($"����:UI_InputManager.AddCloseFunction(uiName = {uiName}, function) : ã������ �̸��� �������� �ʴ� UI �����Դϴ�. �ٽ� Ȯ�� �ٶ��ϴ�.\n���ӿ�����Ʈ�� : {gameObject.name}");
            return;
        }

        UI_Elements[m_foundIndex].CloseUI += function;
        UI_Elements[m_foundIndex].hasInitializationFunction = true;
    }


    void Awake()
    {
        foreach (var element in UI_Elements)
        {
            if (element.uiObject != null)
            {
                element.uiObject.SetActive(false);
            }
        }    
    }

    void Update()
    {
        InputGetKey();
    }

    void InputGetKey()
    {
        foreach (var element in UI_Elements)
        {
            if (Input.GetKeyDown(element.toggleKey))
            {
                element.isActive = !element.isActive;
                if (element.uiObject != null)
                {
                    element.uiObject.SetActive(!element.uiObject.activeSelf);
                }
                else
                {
                    if (isDebugGameObjectNotExist)
                    {
                        Debug.Log("UI_InputManager.InputGetKey() : ���ӿ�����Ʈ�� �������� ����");
                    }
                }
                DoAction(element);

            }
        }
    }

    private int M_FindIndex(string uiName)
    {
        int m_result = -1;
        // UI�� ������ 1000�� �̳��̹Ƿ� O(N) Ž���� �����մϴ�.
        for (int m_index = 0; m_index < UI_Elements.Length; ++m_index)
        {
            if (uiName.Equals(UI_Elements[m_index].identificationName))
            {
                m_result = m_index;
            }
        }
        return m_result;
    }

    private void DoAction(UI_Element target)
    {
        if (target.hasInitializationFunction == false)
        {
            return;
        }

        if (target.uiObject.activeSelf)
        {
            target.InitializeUI();
        }
        else
        {
            target.CloseUI();
        }
    }
}
