using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InventoryController : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private TestItem[] items = new TestItem[7];

    [Header("Slots")]
    [SerializeField] private InventorySlot[] slots;
    [SerializeField] private int curSlotCount;

    [Header("Inventory")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject inventoryBasePanel;

    [Header("External Scripts")]
    [SerializeField] private DataController dataController;



    private void OnEnable()
    {
        curSlotCount = PlayerData.instance.items.Length;

        for (int i = 0; i < 10; i++)
        {
            slots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < PlayerData.instance.items.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// TODO:
    /// 실행해보면 Current Item Slots Count가 순간적으로 다르게 찍히는 경우가 있음,
    /// 나중에 버그를 일으킬 것으로 보여 꼭 수정이 필요함 (안 생긴다면 천만다행이지만..)
    /// 현재 SetSlotScale의 return문을 주석처리하면 나중에 예상되는 버그가 발생함
    /// </summary>
    private void Update()
    {
        Debug.Log("Current Item Slots Count : " + curSlotCount);
        Debug.Log("Slots Length : " + slots.Length);

        UpdateSlot();
        SetSlotScale();
    }


    private void UpdateSlot()
    {
        for (int i = 0; i < PlayerData.instance.items.Length; i++)
        {
            for (int j = 0; j < items.Length; j++)
            {
                if (PlayerData.instance.items[i] == items[j].name)
                {
                    AcquireItem(items[j]);
                }
            }
        }
    }

    private void SetSlotScale()
    {
        if (curSlotCount <= 5)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 300);

            #region Set Position Y
            for (int i = 0; i < curSlotCount; i++)
            {
                RectTransform slotRectTransform = slots[i].GetComponent<RectTransform>();
                Vector2 newPosition = new Vector2(slotRectTransform.anchoredPosition.x, 10);
                slotRectTransform.anchoredPosition = newPosition;
            }
            #endregion

            for (int i = 5; i < 10; i++)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
        else
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 500);

            #region Set Position Y
            for (int i = 0; i < 5; i++)
            {
                RectTransform slotRectTransform = slots[i].GetComponent<RectTransform>();
                Vector2 newPosition = new Vector2(slotRectTransform.anchoredPosition.x, 80);
                slotRectTransform.anchoredPosition = newPosition;
            }

            for (int i = 5; i < curSlotCount; i++)
            {
                RectTransform slotRectTransform = slots[i].GetComponent<RectTransform>();
                Vector2 newPosition = new Vector2(slotRectTransform.anchoredPosition.x, -70);
                slotRectTransform.anchoredPosition = newPosition;

                slots[i].gameObject.SetActive(true);
            }
            #endregion

            for (int i = curSlotCount; i < 10; i++)
            {
                slots[i].gameObject.SetActive(false);
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
                slots[i].AddItemSlot(item, count);

                if (slots[i].gameObject.activeSelf == false)
                {
                    curSlotCount++;
                }
                return;
            }
        }
    }
}