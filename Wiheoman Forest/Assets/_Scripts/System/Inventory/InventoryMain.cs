using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryMain : InventoryBase
{
    [SerializeField] private InventorySlot[] mSlots;

    private bool isInventoryActive = false;

    new void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        OpenInventory();
    }

    public void AcquireItem(TestItem item, int count = 1)
    {       
        if (item.CanOverlap)
        {
            for(int i = 0; i < mSlots.Length; i++)
            {
                if (mSlots[i].Item != null && mSlots[i].Item.ItemID == item.ItemID)
                {
                    mSlots[i].ItemCountUpdate(count);
                    return;
                }
            }
        }

        // 중첩 안되면 빈 슬롯
        for (int i = 0; i < mSlots.Length; i++)
        {
            if (mSlots[i].Item == null)
            {
                mSlots[i].AddItem(item, count);
                Debug.Log("아이템 추가");
                return;
            }
        }
    }

    private void OpenInventory()
    {
        if(Input.GetKeyUp(KeyCode.D))
        {
            if(!isInventoryActive)
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
