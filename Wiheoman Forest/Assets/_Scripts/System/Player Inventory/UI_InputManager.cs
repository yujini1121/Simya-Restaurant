using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_Element
{
    public static UI_Element instance; 

    public GameObject uiObject;
    public bool isActive;
    public KeyCode toggleKey;

    public UI_Element() { instance = this; }
}

public class UI_InputManager : MonoBehaviour
{
    [SerializeField] private UI_Element[] UI_Elements;

    void Awake()
    {
        foreach (var element in UI_Elements)
        {
            element.uiObject.SetActive(false);
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
                element.uiObject.SetActive(!element.uiObject.activeSelf);
            }
        }
    }
}
