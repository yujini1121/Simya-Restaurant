using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemShopSlotInfo
{
    [Header("판매할 아이템")]
    [SerializeField] public TestItem sellItem;

    [Header("거래 한 번 당 아이템 비용")]
    [SerializeField] public int cost;

    [Header("아이템의 재고량")]
    [SerializeField] public int ItemAmount;

    [Header("거래 1회당 아이템 지급 개수")]
    [SerializeField] public int GiveItemAmount;

    public ItemShopSlotInfo(ItemShopSlotInfo origin)
    {
        this.sellItem = origin.sellItem;
        this.cost = origin.cost;
        this.ItemAmount = origin.ItemAmount;
        this.GiveItemAmount = origin.GiveItemAmount;
    }
}

public class ItemShopSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mName, mCost;
    [SerializeField] private Button mBuyButton;

    private ItemShopSlotInfo mSellInfo;

    public void InitSlot(ItemShopSlotInfo sellItem)
    {
        // 정보 가져오기
        mSellInfo = sellItem;

        mName.text = mSellInfo.sellItem.name;
        mCost.text = mSellInfo.cost.ToString();
    }

    public void BuyItem()
    {

    }
}
