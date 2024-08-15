using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestShopManager : MonoBehaviour
{
    [Header("판매할 아이템")]
    [SerializeField] private TestItem[] sellItem;

    [Header("상점 ui")]
    [SerializeField] private GameObject storeUI;

    [Header("상점 판매 ui")]
    [SerializeField] private GameObject storeUIPrefab;

    [Header("상점 판매 ui가 배치될 부모 객체")]
    [SerializeField] private Transform storeUIParent;

    [Header("상점 아이템 설명")]
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    private bool isStoreActive = false;
    private int selectedIndex = 0;
    private GameObject selectedItemUI;
    private TestItem selectedItem;
    private GameObject newItemUI;

    [System.Serializable]
    public class ItemInfomation
    {
        public int itemID;
        public string Description;
        public string Price;
    }

    [System.Serializable]
    public class ItemInformationList
    {
        public ItemInfomation[] ItemDescription;
    }

    private ItemInformationList itemInfo;

    void Start()
    {
        itemInfo = JsonUtility.FromJson<ItemInformationList>(Resources.Load<TextAsset>("Json Files/TestItemDescription").text);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isStoreActive = !isStoreActive;
            storeUI.SetActive(isStoreActive);

            if (isStoreActive)
            {
                InitSlot();
                SetItemInfo();
                SelectItem(0);
            }
        }

        if (isStoreActive)
        {
            SelectInput();
        }
    }

    private void SelectInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }
    }

    private void ChangeSelection(int direction)
    {
        selectedIndex = Mathf.Clamp(selectedIndex + direction, 0, storeUIParent.childCount - 1);

        GameObject itemUI = storeUIParent.GetChild(selectedIndex).gameObject;

        if (selectedItemUI != null)
        {
            ToggleOutline(selectedItemUI, false);
        }

        ToggleOutline(itemUI, true);
        selectedItemUI = itemUI;

        selectedItem = sellItem[selectedIndex];
        Transform backgroundPanel = storeUI.transform.Find("Store_BackGroundPanel");
        Transform itemImagePanel = backgroundPanel.transform.Find("Item_ImagePanel");
        Image itemImage = itemImagePanel.transform.Find("Image").GetComponent<Image>();
        itemImage.sprite = selectedItem.ItemImage;

        SetItemInfo();
    }

    private void ToggleOutline(GameObject itemUI, bool enable)
    {
        var outline = itemUI.GetComponent<Outline>();
        if(outline != null)
        {
            outline.enabled = enable;
        }
    }

    private void SelectItem(int index)
    {
        if (storeUIParent != null && storeUIParent.childCount > 0)
        {
            selectedIndex = Mathf.Clamp(index, 0, storeUIParent.childCount - 1);
            selectedItemUI = storeUIParent.GetChild(selectedIndex).gameObject;
            ToggleOutline(selectedItemUI, true);
        }
    }

    private void InitSlot()
    {
        foreach (Transform child in storeUIParent)
        {
            Destroy(child.gameObject);
        }

        foreach(var item in sellItem)
        {
            newItemUI = Instantiate(storeUIPrefab, storeUIParent);

            var itemData = FindItemData(item.ItemID);
            if (itemData != null)
            {
                newItemUI.transform.Find("ItemName_Text").GetComponent<TextMeshProUGUI>().text = item.ItemName;
                newItemUI.transform.Find("ItemAmount_Text").GetComponent<TextMeshProUGUI>().text = itemData.Price;
            }
        }
    }    

    private void SetItemInfo()
    {
        int currentSelectedItemId = selectedItem.ItemID;

        var itemData = FindItemData(currentSelectedItemId);
        if (itemData != null)
        {
            itemDescriptionText.text = itemData.Description;
        }
    }

    private ItemInfomation FindItemData(int itemID)
    {
        foreach (var item in itemInfo.ItemDescription)
        {
            if (item.itemID == itemID)
            {
                return item;
            }
        }
        return null;
    }
}
