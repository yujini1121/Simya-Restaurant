using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InventoryController : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private GameObject inventoryBase;
    [SerializeField] private GameObject inventoryBasePanel;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] private bool isInventoryActive = false;

    [Header("Slots")]
    [SerializeField] private InventorySlot[] slots;

    [Header("External Scripts")]
    [SerializeField] private DataController dataController;
    [SerializeField] private Player_Inventory playerInventory;

    private int curSlotCount;
    TestItem[] items = new TestItem[7];


    /// <summary>
    /// Get Items List
    /// </summary>
    private void Start()
    {
        playerInventory = GetComponent<Player_Inventory>();

        items[0] = playerInventory.item1;
        items[1] = playerInventory.item2;
        items[2] = playerInventory.item3;
        items[3] = playerInventory.item4;
        items[4] = playerInventory.item5;
        items[5] = playerInventory.item6;
        items[6] = playerInventory.item7;
    }

    private void OnEnable()
    {
        curSlotCount = PlayerData.instance.items.Length;

        for (int i = 0; i < 10; i++)
        {
            slots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < curSlotCount; i++)
        {
            slots[i].gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// TODO:
    /// 실행해보면 Current Item Slots Count가 순간적으로 다르게 찍히는 경우가 있음,
    /// 나중에 버그를 일으킬 것으로 보여 꼭 수정이 필요함 (안 생긴다면 천만다행이지만..)
    /// 현재 SetSlotScale의 return문을 주석처리하면 나중에 예상되는 버그가 발생함
    /// </summary>
    private void Update()
    {
        Debug.Log("Current Item Slots Count : " + curSlotCount);
        Debug.Log("Slots Length : " + slots.Length);

        UpdateSlot();
        SetSlotScale();
    }


    private void UpdateSlot()
    {
        for (int i = 0; i < PlayerData.instance.items.Length; i++)
        {
            for (int j = 0; j < items.Length; j++)
            {
                if (PlayerData.instance.items[i] == items[j].name)
                {
                    AcquireItem(items[j]);
                }
            }
        }
    }

    private void SetSlotScale()
    {
        if (curSlotCount != slots.Length)
        {
            return;
        }

        if (curSlotCount <= 5)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 200);

            for (int i = 5; i < 10; i++)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 400);

            for (int i = 5; i < 10; i++)
            {
                slots[i].gameObject.SetActive(false);
            }
            for (int i = 5; i < curSlotCount; i++)
            {
                slots[i].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// ????? 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void AcquireItem(TestItem item, int count = 1)
    {
        if (item.CanOverlap)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].Item != null && slots[i].Item.ItemID == item.ItemID)
                {
                    slots[i].ItemCountUpdate(count);
                    return;
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == null)
            {
                curSlotCount++;
                slots[i].AddItemSlot(item, count);
                return;
            }
        }
    }
}