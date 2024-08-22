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
        Debug.Log("Current Item Slots Count : " + curSlotCount);

        for (int i = 0; i < 10; i++)
        {
            if (i < curSlotCount)
            {
                slots[i].gameObject.SetActive(true);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        UpdateSlot();
        SetSlotScale();
    }


    private void UpdateSlot()
    {
        if (curSlotCount <= 10 && curSlotCount > 0)
        {
            slots[curSlotCount - 1].gameObject.SetActive(true);
        }

        for (int i = 0; i < PlayerData.instance.items.Length; i++)
        {
            for (int j = 0; j < items.Length; j++)
            {
                if (PlayerData.instance.items[i] == items[j].name)
                {
                    Debug.Log(items[j]);
                    AcquireItem(items[j]);
                }
            }
        }
    }

    //private void AcquireItem(TestItem item)
    //{

    //}

    #region 잠시 접어둠
    private void SetSlotScale()
    {
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
    #endregion

}