using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UI_InputManager;

[System.Serializable]
public class UI_Element
{
    public GameObject obj;
    public bool isActive;
    public KeyCode toggleKey;
}

public class InventoryBase : MonoBehaviour
{
    [SerializeField] private UI_Element[] UI_Elements;

    [SerializeField] private GameObject playerInventory;
    [SerializeField] private GameObject chestInventory;

    void Awake()
    {
        if (playerInventory.activeSelf) { playerInventory.SetActive(false); }
        if (chestInventory.activeSelf) { chestInventory.SetActive(false); }
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
                element.obj.SetActive(!element.obj.activeSelf);
            }
        }
    }
}