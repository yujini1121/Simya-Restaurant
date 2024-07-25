using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // 아이템 인스턴스
    private TestItem mItem;
    public TestItem Item
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

    // 인벤토리에 새로운 아이템 슬롯 추가
    public void AddItem(TestItem item, int count = 1)
    {
        mItem = item;
        mItemCount = count;
        mItemImage.sprite = mItem.ItemImage;
        Debug.Log("이미지 추가");

        mItemCountText.text = mItemCount.ToString();
    }

    // 아이템 개수 증가
    public void ItemCountUpdate(int count)
    {
        mItemCount += count;
        mItemCountText.text = mItemCount.ToString();

        if(mItemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 슬롯 삭제
    public void ClearSlot()
    {
        mItem = null;
        mItemCount = 0;
        mItemImage.sprite = null;

        mItemCountText.text = "";
    }
}
