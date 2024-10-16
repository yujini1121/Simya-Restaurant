using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public string itemAmount;
}

[System.Serializable]
public class ItemDataList
{
    public List<Item> Item;
}

[System.Serializable]
public class ItemDescription
{
    public int id;
    public string description;
}

[System.Serializable]
public class InventoryItemList
{
    public List<ItemDescription> itemDescription;
}

public class DroppedItemController : MonoBehaviour
{
    [SerializeField] private ItemAttribute itemAttribute;

    private ItemDataList itemDataList;
    private InventoryItemList inventoryList;

    private int ID;
    private string Description;

    private string itemTestPath;
    private string playerInventoryItemPath;

    void Start()
    {
        ReadJsonFile();        
    }

    private void ReadJsonFile()
    {
        itemTestPath = Application.dataPath + "/Resources/Json Files/ItemTest.json";
        playerInventoryItemPath = Application.dataPath + "/Resources/Json Files/PlayerInventoryItems.json";

        if (File.Exists(itemTestPath))
        {
            string itemJson = File.ReadAllText(itemTestPath);
            itemDataList = JsonUtility.FromJson<ItemDataList>(itemJson);
        }
        else
        {
            Debug.Log("ItemTest.json을 찾지 못 함");
            return;
        }

        if (File.Exists(playerInventoryItemPath))
        {
            string inventoryJson = File.ReadAllText(playerInventoryItemPath);
            inventoryList = JsonUtility.FromJson<InventoryItemList>(inventoryJson);
        }
        else
        {
            // 파일이 없을 경우 새로운 리스트를 생성
            inventoryList = new InventoryItemList();
        }
    }

    private void UpdateJson()
    {
        Item matchedItem = null;
        Debug.Log("왜 안되냐 죽을ㄷ래");
        foreach (var item in itemDataList.Item)
        {
            Debug.Log("반복문");
            if (itemAttribute.ItemID == item.itemID)
            {
                Debug.Log("if문");
                matchedItem = item;
                break;
            }
        }

        if(matchedItem != null)
        {
            ID = matchedItem.itemID;
            Debug.Log("ItemId : " + ID);
            Debug.Log("ItemId2 : " + matchedItem.itemID);

            if (itemAttribute.ItemID == ID)
            {
                Description = matchedItem.itemDescription;
                Debug.Log("ItemDescription : " + Description);
                Debug.Log("ItemDescription2 : " + matchedItem.itemDescription);

                ItemDescription newItem = new ItemDescription
                {
                    id = ID,
                    description = Description
                };

                inventoryList.itemDescription.Add(newItem);

                string updateInventoryJson = JsonUtility.ToJson(inventoryList, true);
                File.WriteAllText(playerInventoryItemPath, updateInventoryJson);
                Debug.Log("PlayerInventoryItems.json 파일 업데이트");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("플레이어랑 충돌!");

#warning 나중에 플레이어 인벤토리 접근해서 그곳에 아이템 추가할 것
        UpdateJson();
        Debug.Log($"여기서 아이템을 수집했습니다");

        Destroy(gameObject);
    }
}
