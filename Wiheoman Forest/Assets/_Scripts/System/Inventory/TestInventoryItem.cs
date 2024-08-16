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

    private void Start()
    {
        // 버튼 클릭 이벤트에 메서드 직접 연결
        item1Button.onClick.AddListener(() => AcquireItem(item1, "1번"));
        item2Button.onClick.AddListener(() => AcquireItem(item2, "2번"));
        item3Button.onClick.AddListener(() => AcquireItem(item3, "3번"));
    }

    private void AcquireItem(TestItem item, string debugMessage)
    {
        mInventoryMain.AcquireItem(item);
        Debug.Log(debugMessage);
    }
}
