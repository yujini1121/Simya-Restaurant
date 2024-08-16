using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] protected GameObject inventoryBase;
    [SerializeField] protected GameObject inventoryBasePanel;
    [SerializeField] private bool isInventoryActive = false;

    [Header("Slots")]
    [SerializeField] private InventorySlot[] slots;

    [Header("External Scripts")]
    [SerializeField] private GameObject dataController;
    [SerializeField] private int curItemCount;


    private void Start()
    {
        if (inventoryBase.activeSelf)
        {
            inventoryBase.SetActive(false);
        }

        curItemCount = PlayerData.instance.items.Length;
        slots = new InventorySlot[10];
    }

    void Update()
    {
        OpenInventory();
    }


    public void AcquireItem(TestItem item, int count = 1)
    {
        if (item.CanOverlap && curItemCount == slots.Length)
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
                slots[i].AddItemSlot(item, count);
                Debug.Log("아이템 슬롯 추가");
                return;
            }
        }
    }

    private void OpenInventory()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (!isInventoryActive)
            {
                inventoryBase.SetActive(true);
                isInventoryActive = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                inventoryBase.SetActive(false);
                isInventoryActive = false;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
