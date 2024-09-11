using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    [Header("판매할 아이템")]
    [SerializeField] private ItemAttribute[] sellItem;

    [Header("상점 ui")]
    [SerializeField] private GameObject storeUI;

    [Header("상점 판매 ui")]
    [SerializeField] private GameObject storeUIPrefab;

    [Header("상점 판매 ui가 배치될 부모 객체")]
    [SerializeField] private Transform storeUIParent;

    [Header("상점 아이템 설명")]
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    [Header("플레이어 소지금 Text UI")]
    [SerializeField] private TextMeshProUGUI playerGoldText;

    [Header("구매 금액 합계 Text UI")]
    [SerializeField] private TextMeshProUGUI totalPayAmountText;

    [Header("스크롤 컴포넌트")]
    [SerializeField] private ScrollRect scrollRect; //

    private int playerGold;
    private int selectedIndex = 0;
    private int sumPayAmount = 0;
    private bool isStoreActive = false;

    private int buyCount = 0;
    private int buyAmount = 0;
    private TextMeshProUGUI buyCountText;
    private TextMeshProUGUI buyAmountText;
    private Dictionary<int, int> itemBuyCount = new Dictionary<int, int>();
    private Dictionary<int, int> itemBuyAmount = new Dictionary<int, int>();

    private GameObject selectedItemUI;
    private ItemAttribute selectedItem;
    private GameObject newItemUI;
    private ItemInformationList itemInfo;
    private PlayerData playerGoldData;
    private DataController dataController;

    [System.Serializable]
    public class ItemInfomation
    {
        public int itemID;
        public string Description;
        public int Price;
    }

    [System.Serializable]
    public class ItemInformationList
    {
        public ItemInfomation[] ItemDescription;
    }


    void Start()
    {
        JsonFileReadAndGoldSet();
        GameObject dataControllerObject = GameObject.Find("Data Controller");
        dataController = dataControllerObject.GetComponent<DataController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isStoreActive = !isStoreActive;
            storeUI.SetActive(isStoreActive);

            if (isStoreActive)
            {
                InitSlot();
                SelectItem(0);
                SetItemInfo();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            isStoreActive = false;
            storeUI.SetActive(isStoreActive);
        }

        if (isStoreActive)
        {
            SelectInput();

            BuyItem();
        }
    }

    private void JsonFileReadAndGoldSet()
    {
        itemInfo = JsonUtility.FromJson<ItemInformationList>(Resources.Load<TextAsset>("Json Files/TestItemDescription").text);
        playerGoldData = JsonUtility.FromJson<PlayerData>(Resources.Load<TextAsset>("Json Files/PlayerData").text);
        
        playerGold = playerGoldData.gold;

        playerGoldText.text = playerGold.ToString() + " $";
        totalPayAmountText.text = sumPayAmount.ToString() + " $";        
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

        //
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            BuyCount(KeyCode.RightArrow);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            BuyCount(KeyCode.LeftArrow);
        }
    }

    private void BuyCount(KeyCode keycode)
    {
        var itemData = FindItemData(selectedItem.ItemID);

        if (!itemBuyCount.ContainsKey(selectedItem.ItemID))
        {
            itemBuyCount[selectedItem.ItemID] = 0;
            itemBuyAmount[selectedItem.ItemID] = 0;
        }

        switch(keycode)
        {
            case KeyCode.RightArrow:
                itemBuyCount[selectedItem.ItemID]++;
                itemBuyAmount[selectedItem.ItemID] += itemData.Price;
                break;
            case KeyCode.LeftArrow:
                if(itemBuyCount[selectedItem.ItemID] > 0)
                {
                    itemBuyCount[selectedItem.ItemID]--;
                    itemBuyAmount[selectedItem.ItemID] -= itemData.Price;
                }                
                break;
        }

        UiUpdate();
    }

    private void UiUpdate()
    {
        buyCountText.text = itemBuyCount[selectedItem.ItemID].ToString() + " 개";
        buyAmountText.text = itemBuyAmount[selectedItem.ItemID].ToString() + " $";
    }

    private void UpdateItemImage()
    {
        if(selectedItem != null)
        {
            Transform backgroundPanel = storeUI.transform.Find("Store_BackGroundPanel");
            Transform itemImagePanel = backgroundPanel.transform.Find("Item_ImagePanel");

            Image itemImage = itemImagePanel.transform.Find("Image").GetComponent<Image>();
            itemImage.sprite = selectedItem.ItemImage;
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
        
        buyCountText = selectedItemUI.transform.Find("BuyCount_Text").GetComponent<TextMeshProUGUI>();
        buyAmountText = selectedItemUI.transform.Find("BuyAmount_Text").GetComponent<TextMeshProUGUI>();

        UpdateItemImage();

        SetItemInfo();

        ScrollPosition();
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
            selectedItem = sellItem[selectedIndex];
        }
    }

    private void InitSlot()
    {
        sumPayAmount = 0;
        totalPayAmountText.text = sumPayAmount.ToString() + " $";

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
                newItemUI.transform.Find("ItemAmount_Text").GetComponent<TextMeshProUGUI>().text = itemData.Price.ToString();
            }
            
            buyCountText = newItemUI.transform.Find("BuyCount_Text").GetComponent<TextMeshProUGUI>();
            buyAmountText = newItemUI.transform.Find("BuyAmount_Text").GetComponent<TextMeshProUGUI>();

            buyCountText.text = buyCount.ToString() + " 개";
            buyAmountText.text = buyAmount.ToString() + " $";            
        }

        if(sellItem.Length > 0)
        {
            SelectItem(0);
            SetItemInfo();
            UpdateItemImage();
        }
    }    

    private void BuyItem()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var itemData = FindItemData(selectedItem.ItemID);

            int totalPrice = itemBuyCount.ContainsKey(selectedItem.ItemID) ? itemBuyAmount[selectedItem.ItemID] : itemData.Price;

            if (playerGold >= itemData.Price)
            {
                playerGold -= totalPrice;
                sumPayAmount += totalPrice;

                totalPayAmountText.text = sumPayAmount.ToString() + " $";
                playerGoldText.text = playerGold.ToString() + " $";

                playerGoldData.gold = playerGold;
                dataController.SaveData();

                itemBuyCount[selectedItem.ItemID] = 0;
                itemBuyAmount[selectedItem.ItemID] = 0; //

                UiUpdate();

            }
            else
            {
                Debug.Log("소지금 부족!!");        
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

    private void ScrollPosition()
    {
        // 선택된 아이템이 스크롤 뷰의 마지막 부분에 도달했을 때 스크롤을 이동
        float selectedItemYPos = selectedIndex / (float)(storeUIParent.childCount - 1);

        // 마지막 아이템에 도달할 때만 스크롤을 내림
        if (selectedIndex == storeUIParent.childCount - 1)
        {
            scrollRect.verticalNormalizedPosition = 0f;  // 가장 아래로 스크롤
        }
        else if(selectedIndex == 0)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
        else if (selectedItemYPos > 0.9f)  // 하단 20% 근처에 도달했을 때 스크롤
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(1 - selectedItemYPos, 0f, 1f);
        }
        // 상단 20% 근처에 도달했을 때 스크롤을 올림
        else if (selectedItemYPos < 0.1f)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(1 - selectedItemYPos, 0f, 1f);
        }
    }


}
