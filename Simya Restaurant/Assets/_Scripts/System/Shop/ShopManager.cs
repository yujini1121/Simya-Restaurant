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
    [SerializeField] private ScrollRect scrollRect;

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
        if (Input.GetKeyDown(KeyCode.D))
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


    // =========================================================
    // Json 파일에서 아이템 설명 및 플레이어 데이터 로드,
    // 플레이어 골드 값 초기화
    // =========================================================
    private void JsonFileReadAndGoldSet()
    {
        itemInfo = JsonUtility.FromJson<ItemInformationList>(Resources.Load<TextAsset>("Json Files/TestItemDescription").text);
        playerGoldData = JsonUtility.FromJson<PlayerData>(Resources.Load<TextAsset>("Json Files/PlayerData").text);
        
        playerGold = playerGoldData.gold;

        playerGoldText.text = playerGold.ToString() + " $";
        totalPayAmountText.text = sumPayAmount.ToString() + " $";        
    }

    // =========================================================
    // 방향키 눌렀을 때 선택 변경 처리 
    // =========================================================
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

    // =========================================================
    // 선택된 아이템 이미지 ui 업데이트 
    // =========================================================
    private void UpdateImage()
    {
        if(selectedItem != null)
        {
            Transform backgroundPanel = storeUI.transform.Find("Store_BackGroundPanel");
            Transform itemImagePanel = backgroundPanel.transform.Find("Item_ImagePanel");
            Image itemImage = itemImagePanel.transform.Find("Image").GetComponent<Image>();

            itemImage.sprite = selectedItem.ItemImage;
        }
    }

    // =========================================================
    // 선택된 아이템 슬롯 변경
    // 아웃라인 처리와 아이템 이미지, 설명을 업데이트
    // =========================================================
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

        UpdateImage();

        SetItemInfo();

        ScrollPosition();
    }

    // =========================================================
    // 아이템 슬롯 아웃라인을 활성화 / 비활성화
    // =========================================================
    private void ToggleOutline(GameObject itemUI, bool enable)
    {
        var outline = itemUI.GetComponent<Outline>();
        if(outline != null)
        {
            outline.enabled = enable;
        }
    }

    // ==========================================================================================
    // 아이템 슬롯 선택, 선택된 슬롯의 아웃라인을 활성화하고 선택된 아이템 데이터를 selectedItem에 저장
    // ==========================================================================================
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

    // =========================================================
    // 상점 슬롯을 초기화 하고 슬롯들을 생성
    // 아이템 이름, 가격, 구매 금액, 구매 갯수 등 표시
    // =========================================================
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
            UpdateImage();
        }
    }

    // =========================================================
    // 구매를 선택한 아이템의 구매 수량과 금액 업데이트
    // =========================================================
    private void UiUpdate()
    {
        buyCountText.text = itemBuyCount[selectedItem.ItemID].ToString() + " 개";
        buyAmountText.text = itemBuyAmount[selectedItem.ItemID].ToString() + " $";
    }

    // =========================================================
    // 아이템 구매 처리
    // 오른쪽 방향키로 구매 갯수를 늘리고
    // 왼쪽 방향키로 구매 갯수를 줄임
    // F 키로 선택된 아이템 모두 구매
    // 구매 후 수량 및 금액 초기화, 플레이어 데이터 저장
    // =========================================================
    private void BuyItem()
    {
        var itemData = FindItemData(selectedItem.ItemID);

        if (!itemBuyCount.ContainsKey(selectedItem.ItemID))
        {
            itemBuyCount[selectedItem.ItemID] = 0;
            itemBuyAmount[selectedItem.ItemID] = 0;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            itemBuyCount[selectedItem.ItemID]++;
            itemBuyAmount[selectedItem.ItemID] += itemData.Price;
            UiUpdate();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (itemBuyCount[selectedItem.ItemID] > 0)
            {
                itemBuyCount[selectedItem.ItemID]--;
                itemBuyAmount[selectedItem.ItemID] -= itemData.Price;
                UiUpdate();
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            int totalPurchaseCost = 0;
            foreach (var item in itemBuyCount)
            {
                int itemID = item.Key;
                int itemCount = item.Value;
                totalPurchaseCost += itemCount * FindItemData(itemID).Price;
            }

            if (playerGold >= totalPurchaseCost)
            {
                playerGold -= totalPurchaseCost;
                sumPayAmount += totalPurchaseCost;

                totalPayAmountText.text = sumPayAmount.ToString() + " $";
                playerGoldText.text = playerGold.ToString() + " $";

                playerGoldData.gold = playerGold;
                dataController.SaveData();

                foreach (var itemID in itemBuyCount.Keys)
                {
                    itemBuyCount[itemID] = 0;
                    itemBuyAmount[itemID] = 0;
                }
            }
            else
            {
                Debug.Log("소지금 부족!!");
            }
        }
    }

    // =========================================================
    // 선택된 아이템 설명을 ui에 띄움
    // =========================================================
    private void SetItemInfo()
    {
        int currentSelectedItemId = selectedItem.ItemID;
        var itemData = FindItemData(currentSelectedItemId);

        if (itemData != null)
        {
            itemDescriptionText.text = itemData.Description;
        }
    }

    // =========================================================
    // 아이템 ID와 일치하는 아이템 데이터를 찾아 반환
    // =========================================================
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

    // =========================================================
    // 선택된 슬롯에 위치에 맞춰 스크롤 위치 조정
    // =========================================================
    private void ScrollPosition()
    {
        float selectedItemYPos = selectedIndex / (float)(storeUIParent.childCount - 1);

        if (selectedIndex == storeUIParent.childCount - 1)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }
        else if(selectedIndex == 0)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
        else if (selectedItemYPos > 0.9f)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(1 - selectedItemYPos, 0f, 1f);
        }
        else if (selectedItemYPos < 0.1f)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(1 - selectedItemYPos, 0f, 1f);
        }
    }
}
