using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ItemShopSlotInfo
{
    [Header("판매할 아이템")]
    [SerializeField] public ItemShopSlot sellItem;

    [Header("거래 당 아이템 비용")]
    [SerializeField] public int itemCost;

    [Header("지급되는 아이템 수")]
    [SerializeField] public int buyItemCount;

    public ItemShopSlotInfo(ItemShopSlotInfo origin)
    {
        this.sellItem = origin.sellItem;
        this.itemCost = origin.itemCost;
        this.buyItemCount = origin.buyItemCount;
    }
}

public class ItemShopSlot : MonoBehaviour
{
    [SerializeField] private InventorySlot itemSlot;
    [SerializeField] private TextMeshProUGUI itemName, itemCost;
    [SerializeField] private Button buyButton;

    private ItemShopSlotInfo sellItemInfo;
    
    public void InitSlot(ItemShopSlotInfo intfoSellItem)
    {
        sellItemInfo = intfoSellItem;

        itemSlot.ClearSlot();


    }
}
