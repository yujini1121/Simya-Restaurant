using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InputManager : MonoBehaviour
{
    [SerializeField] private GameObject UI_SaveAndLoad;
    [SerializeField] private GameObject UI_PlayerInventory;
    [SerializeField] private GameObject UI_TimeSwitching;


    void Start()
    {
        UI_SaveAndLoad.SetActive(false);
        UI_PlayerInventory.SetActive(false);
        UI_TimeSwitching.SetActive(false);
    }

    /// <summary>
    /// switch - case문으로 구현하고 싶었으나, 
    /// 대문자 / 소문자 구별하여 케이스를 작성해야 한다고 하여 임시로 if문으로 작성했습니다.
    /// </summary>
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //    UI_SaveAndLoad.SetActive(true);

        //else if (Input.GetKeyDown(KeyCode.B))
        //    UI_PlayerInventory.SetActive(true);

        //else if (Input.GetKeyDown(KeyCode.X))
        //    UI_TimeSwitching.SetActive(true);

    }
}
