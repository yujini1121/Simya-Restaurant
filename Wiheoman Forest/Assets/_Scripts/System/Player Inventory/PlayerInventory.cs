using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    #region Variable
    [Header("인벤토리 메인")]
    [SerializeField] private PlayerInventoryController mInventoryMain;

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
    [SerializeField] private Button minusButton;
    #endregion


    private void Start()
    {
        // 버튼 클릭 이벤트에 메서드 직접 연결
        item1Button.onClick.AddListener(() => AcquireItem(item1, "Strawberry"));
        item2Button.onClick.AddListener(() => AcquireItem(item2, "Peach"));
        item3Button.onClick.AddListener(() => AcquireItem(item3, "Grape"));
        item4Button.onClick.AddListener(() => AcquireItem(item4, "Orange"));
        item5Button.onClick.AddListener(() => AcquireItem(item5, "Blueberry"));
        item6Button.onClick.AddListener(() => AcquireItem(item6, "Pasnip"));
        item7Button.onClick.AddListener(() => AcquireItem(item7, "Starfruit"));

        //minusButton.onClick.AddListener(() => LoseItem(item1));
    }

    private void AcquireItem(TestItem item, string debugMessage)
    {
        mInventoryMain.AcquireItem(item);
        Debug.Log(debugMessage);
    }

    private void LoseItem(TestItem item)
    {
        
    }
}
