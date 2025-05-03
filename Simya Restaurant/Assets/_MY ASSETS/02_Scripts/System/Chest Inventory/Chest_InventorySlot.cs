using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour

{
    /// <summary>
    /// 아이템 인스턴스 
    /// </summary>
    private ItemAttribute mItem;
    public ItemAttribute Item
    {
        get
        {
            return mItem;
        }
    }

    private int mItemCount;

    [Header("아이템 슬롯 UI 오브젝트")]
    [SerializeField] private Image mItemImage;
    [SerializeField] private TextMeshProUGUI mItemCountText;

    /// <summary>
    /// 인벤토리에 새로운 아이템 슬롯 추가
    /// </summary>
    /// <param name="item"> 해당 아이템</param>
    /// <param name="count"> 해당 아이템을 증가시킬 수</param>
    public void AddItemSlot(ItemAttribute item, int count = 1)
    {
        mItem = item;
        mItemCount = count;
        mItemImage.sprite = mItem.ItemImage;

        mItemCountText.text = mItemCount.ToString();
    }

    /// <summary>
    /// 아이템 개수 증가 
    /// </summary>
    /// <param name="count"></param>
    public void ItemCountUpdate(int count)
    {
        if (mItemCount <= 0)
        {
            ClearSlot();
        }

        mItemCount += count;
        mItemCountText.text = mItemCount.ToString();
    }

    /// <summary>
    /// 슬롯 삭제
    /// </summary>
    public void ClearSlot()
    {
        mItem = null;
        mItemCount = 0;
        mItemImage.sprite = null;

        mItemCountText.text = "";
    }
}
