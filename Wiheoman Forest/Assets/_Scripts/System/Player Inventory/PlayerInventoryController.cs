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
    [SerializeField] RectTransform rectTransform;

    [Header("External Scripts")]
    [SerializeField] private GameObject dataController;
    [SerializeField] private int curItemCount = 0;


    private void Start()
    {
        if (inventoryBase.activeSelf)
        {
            inventoryBase.SetActive(false);
        }

        curItemCount = PlayerData.instance.items.Length;
        rectTransform = inventoryBasePanel.GetComponent<RectTransform>();

        slots[curItemCount].gameObject.SetActive(true);

        for (int i = 0; i < 10; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        OpenInventory();
        SetScale();
        UpdateSlot();
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

    private void SetScale()
    {
        if (curItemCount <= 5)
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
            for (int i = 5; i < curItemCount; i++)
            {
                slots[i].gameObject.SetActive(true);
            }
        }
    }

    private void UpdateSlot()
    {
        if (curItemCount <= 10 && curItemCount > 0)
        {
            slots[curItemCount - 1].gameObject.SetActive(true);
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
                curItemCount++;
                slots[i].AddItemSlot(item, count);

                Debug.Log(curItemCount);
                return;
            }
        }
    }

    public void LoseItem(TestItem item, int count = 1)
    {

    }
}
