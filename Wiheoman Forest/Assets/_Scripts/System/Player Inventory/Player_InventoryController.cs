using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class ItemDescriptions
{
    public List<ItemDescriptionData> itemDescription = new List<ItemDescriptionData>();
}

[System.Serializable]
public class ItemDescriptionData
{
    public static ItemDescriptionData instance;

    public int id;
    public string description;

    public ItemDescriptionData() { instance = this; }
}


public class Player_InventoryController : MonoBehaviour
{
    #region Variables
    [Header("Items")]
    [SerializeField] private ItemAttribute[] items = new ItemAttribute[7];
    [SerializeField] private TextMeshProUGUI descritionText;

    [Header("Slots")]
    [SerializeField] private InventorySlot[] slots;
    [SerializeField] private int curSlotCount;

    [Header("Inventory")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject inventoryBasePanel;

    [Header("External")]
    [SerializeField] private DataController dataController;
    [SerializeField] private ItemDescriptions itemDescriptions;
    private Dictionary<int, string> itemDescriptDict = new Dictionary<int, string>();

    int index = 0;
    int y_Index = 0;
    int x_Index = 0;
    #endregion


    private void Awake()
    {
        string path = Application.dataPath + "/Resources/Json Files/PlayerInventoryItems.json";


        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            itemDescriptions = JsonUtility.FromJson<ItemDescriptions>(data);

            for (int i = 0; i < itemDescriptions.itemDescription.Count; i++)
            {
                itemDescriptDict.Add(itemDescriptions.itemDescription[i].id, itemDescriptions.itemDescription[i].description);
            }
        }
        else
        {
            Debug.LogError("Not Found 'PlayerInventoryItems.json' File.");
        }
    }

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



    private void Update()
    {
        //Debug.Log("Current Item Slots Count : " + curSlotCount);

        SelectInput();
        SelectedItem();

        UpdateSlot();
        SetSlotScale();
    }

    #region Input & Select Item
    private void SelectInput()
    {
        if      (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Y_ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Y_ChangeSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            X_ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            X_ChangeSelection(1);
        }
    }

    private void Y_ChangeSelection(int direction)
    {
        y_Index = Mathf.Clamp(y_Index + direction, 0, 1);
    }

    private void X_ChangeSelection(int direction)
    {
        x_Index = Mathf.Clamp(x_Index + direction, 0, 4);
    }

    private void SelectedItem()
    {
        // Outline & Description 

        slots[index].GetComponent<Outline>().enabled = false;

        if (y_Index * 5 + x_Index >= curSlotCount)
        {
            index = curSlotCount - 1;
            x_Index = index - y_Index * 5;
        }
        else
        {
            index = y_Index * 5 + x_Index;
        }

        slots[index].GetComponent<Outline>().enabled = true;
        itemDescriptDict.TryGetValue(index, out string str);
        descritionText.text = str;
    }
    #endregion

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

    public void AcquireItem(ItemAttribute item, int count = 1)
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