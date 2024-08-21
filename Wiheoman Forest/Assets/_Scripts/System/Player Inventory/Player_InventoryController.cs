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
    [SerializeField] private int curSlotCount;


    private void Start()
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

    void Update()
    {
        OpenInventory();
        SetSlotScale();
        UpdateSlot();

        Debug.Log("Current Item Slots Count : " + curSlotCount);
    }

    private void OpenInventory()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    if (!isInventoryActive)
        //    {
        //        inventoryBase.SetActive(true);
        //        isInventoryActive = true;
        //    }
        //    else
        //    {
        //        inventoryBase.SetActive(false);
        //        isInventoryActive = false;
        //    }
        //}
    }

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

    private void UpdateSlot()
    {
        if (curSlotCount <= 10 && curSlotCount > 0)
        {
            slots[curSlotCount - 1].gameObject.SetActive(true);
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

    public void LoseItem(TestItem item, int count = 1)
    {

    }
}
