using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishMenu : InteractiveObjectBase
{
    public static DishMenu instance;

    [SerializeField] GameObject[] Buttons;
    bool isOpen = false;

    public override void DoInteractiveWithThis()
    {
        isOpen = (!isOpen);

        foreach (var button in Buttons)
        {
            button.gameObject.SetActive(isOpen);
        }
    }

    public void CloseMenu()
    {
        isOpen = false;

        foreach (var button in Buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        instance = this;
    }
}
