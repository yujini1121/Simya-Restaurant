using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryType
{
    PlayerInventory,
    ChestInventory
}

public class InventoryBase : MonoBehaviour
{
    //[SerializeField] InventoryType type;


    public void AddItemSlot(ItemAttribute item, int count = 1)
    {
        string itemName = item.ToString();
        int itemCount = count;
    }

    //private void AcquireItem(ItemAttribute item, int count = 1)
    //{
    //    if (item.CanOverlap)
    //    {
    //        for (int i = 0; i < slots.Length; i++)
    //        {
    //            if (slots[i].Item != null && slots[i].Item.ItemID == item.ItemID)
    //            {
    //                slots[i].ItemCountUpdate(count);
    //                return;
    //            }
    //        }
    //    }

    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].Item == null)
    //        {
    //            slots[i].AddItemSlot(item, count);

    //            if (slots[i].gameObject.activeSelf == false)
    //            {
    //                curSlotCount++;
    //            }
    //            return;
    //        }
    //    }
    //}
}