using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInventoryItem : MonoBehaviour
{
    [Header("인벤토리 메인")]
    [SerializeField] private InventoryMain mInventoryMain;

    [Header("획득할 아이템")]
    [SerializeField] private TestItem item1, item2, item3;

    [Header("UI 버튼")]
    [SerializeField] private Button item1Button;
    [SerializeField] private Button item2Button;
    [SerializeField] private Button item3Button;

    private bool item1Clicked = false;
    private bool item2Clicked = false;
    private bool item3Clicked = false;

    private void Start()
    {
        // 버튼 클릭 이벤트에 메서드 연결
        item1Button.onClick.AddListener(OnItem1ButtonClicked);
        item2Button.onClick.AddListener(OnItem2ButtonClicked);
        item3Button.onClick.AddListener(OnItem3ButtonClicked);
    }

    private void Update()
    {
        // 버튼 클릭 이벤트 확인
        if (item1Clicked)
        {
            item1Clicked = false;
            mInventoryMain.AcquireItem(item1);
            Debug.Log("1번");
        }

        if (item2Clicked)
        {
            item2Clicked = false;
            mInventoryMain.AcquireItem(item2);
            Debug.Log("2번");
        }

        if (item3Clicked)
        {
            item3Clicked = false;
            mInventoryMain.AcquireItem(item3);
            Debug.Log("3번");
        }
    }

    public void OnItem1ButtonClicked()
    {
        item1Clicked = true;
    }

    public void OnItem2ButtonClicked()
    {
        item2Clicked = true;
    }

    public void OnItem3ButtonClicked()
    {
        item3Clicked = true;
    }
}
