using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    #region Variable
    [Header("인벤토리 메인")]
    [SerializeField] private InventoryMain mInventoryMain;

    [Header("획득할 아이템")] [Space(10)]
    [SerializeField] private TestItem item1;
    [SerializeField] private TestItem item2;
    [SerializeField] private TestItem item3;
    [SerializeField] private TestItem item4;
    [SerializeField] private TestItem item5;
    [SerializeField] private TestItem item6;
    [SerializeField] private TestItem item7;

    [Header("UI 버튼")] [Space(10)]
    [SerializeField] private Button item1Button;
    [SerializeField] private Button item2Button;
    [SerializeField] private Button item3Button;
    [SerializeField] private Button item4Button;
    [SerializeField] private Button item5Button;
    [SerializeField] private Button item6Button;
    [SerializeField] private Button item7Button;
    #endregion


    private void Start()
    {
        // 버튼 클릭 이벤트에 메서드 직접 연결
        item1Button.onClick.AddListener(() => AcquireItem(item1, "1번"));
        item2Button.onClick.AddListener(() => AcquireItem(item2, "2번"));
        item3Button.onClick.AddListener(() => AcquireItem(item3, "3번"));
        item4Button.onClick.AddListener(() => AcquireItem(item4, "4번"));
        item5Button.onClick.AddListener(() => AcquireItem(item5, "5번"));
        item6Button.onClick.AddListener(() => AcquireItem(item6, "6번"));
        item7Button.onClick.AddListener(() => AcquireItem(item7, "7번"));
    }

    private void AcquireItem(TestItem item, string debugMessage)
    {
        mInventoryMain.AcquireItem(item);
        Debug.Log(debugMessage);
    }
}
