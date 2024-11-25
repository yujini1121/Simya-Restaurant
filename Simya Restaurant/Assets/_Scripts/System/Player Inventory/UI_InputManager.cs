using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

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
    // 특정 UI는 단순히 Active 상태를 바꾸는것만으로 부족할 수 있습니다.
    // UI를 열거나 닫을때, 추가적으로 필요한 함수 작업이 있다면
    // 해당 UI를 컨트롤하는 스크립트에서 Start 메서드에 접근한 뒤,
    // 그곳에서 AddInitializeFunction이나 AddCloseFunction를 호출하시길 바랍니다.
    // 또한 해당 함수를 호출할 때, UI_InputManager의 인스펙터 창에서
    // identificationName와 AddInitializeFunction 혹은 AddCloseFunction의 String 값이
    // 일치한지 항상 확인하시길 바랍니다.
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
            Debug.LogWarning($"오류:UI_InputManager.AddInitializeFunction(uiName = {uiName}, function) : 찾으려는 이름은 존재하지 않는 UI 대상명입니다. 다시 확인 바랍니다.\n게임오브젝트명 : {gameObject.name}");
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
            Debug.LogWarning($"오류:UI_InputManager.AddCloseFunction(uiName = {uiName}, function) : 찾으려는 이름은 존재하지 않는 UI 대상명입니다. 다시 확인 바랍니다.\n게임오브젝트명 : {gameObject.name}");
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
                        Debug.Log("UI_InputManager.InputGetKey() : 게임오브젝트가 지정되지 않음");
                    }
                }
                DoAction(element);

            }
        }
    }

    private int M_FindIndex(string uiName)
    {
        int m_result = -1;
        // UI의 개수가 1000개 이내이므로 O(N) 탐색을 실행합니다.
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
