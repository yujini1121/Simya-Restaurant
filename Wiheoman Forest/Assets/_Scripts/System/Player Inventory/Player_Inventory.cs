using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Inventory : MonoBehaviour
{
    #region Variable
    [Header("Items")] 
    [SerializeField] private TestItem item1;
    [SerializeField] private TestItem item2;
    [SerializeField] private TestItem item3;
    [SerializeField] private TestItem item4;
    [SerializeField] private TestItem item5;
    [SerializeField] private TestItem item6;
    [SerializeField] private TestItem item7;

    [Space(10)]
    [Header("Buttons")] 
    [SerializeField] private Button item1Button;
    [SerializeField] private Button item2Button;
    [SerializeField] private Button item3Button;
    [SerializeField] private Button item4Button;
    [SerializeField] private Button item5Button;
    [SerializeField] private Button item6Button;
    [SerializeField] private Button item7Button;
    [SerializeField] private Button minusButton;

    [Space(10)]
    [Header("External")]
    [SerializeField] private Player_InventoryController mInventoryMain;
    #endregion


    /// <summary>
    /// 버튼 클릭 이벤트에 직접 연결 - Inspector창에서 OnClick 메서드를 사용하려 했으나, 매개변수가 매우 한정적일때만 가능하여 이번 경우엔 어려울 수 있어 직접 연결함.
    /// </summary>
    private void Start()
    {
        item1Button.onClick.AddListener(() => AcquireItem(item1, "Add Strawberry"));
        item2Button.onClick.AddListener(() => AcquireItem(item2, "Add Peach"));
        item3Button.onClick.AddListener(() => AcquireItem(item3, "Add Grape"));
        item4Button.onClick.AddListener(() => AcquireItem(item4, "Add Orange"));
        item5Button.onClick.AddListener(() => AcquireItem(item5, "Add Blueberry"));
        item6Button.onClick.AddListener(() => AcquireItem(item6, "Add Pasnip"));
        item7Button.onClick.AddListener(() => AcquireItem(item7, "Add Starfruit"));

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
