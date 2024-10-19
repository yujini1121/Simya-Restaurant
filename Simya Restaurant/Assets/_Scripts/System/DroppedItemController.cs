using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// =========================================================
// 아이템 정보 Json 파일 
// =========================================================
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

// =========================================================
// 플레이어 인벤토리 정보 Json 파일 
// =========================================================
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

// =========================================================
// 플레이어 정보 Json 파일 
// =========================================================
[System.Serializable]
public class GamePlayerData
{
    public bool isDead;
    public string name;
    public int level;
    public int gold;
    public List<string> items;
}

public class DroppedItemController : MonoBehaviour
{
    [SerializeField] private ItemAttribute itemAttribute;

    private ItemDataList itemDataList;
    private InventoryItemList inventoryList;
    private GamePlayerData playerData;

    private DataController dataController;

    private int ID;
    private string Description;

    private string itemDataPath;
    private string playerInventoryItemPath;
    private string playerDataPath;

    private void Start()
    {
        GameObject dataControllerObject = GameObject.Find("Data Controller");
        dataController = dataControllerObject.GetComponent<DataController>();
    }

    private void ReadJsonFile()
    {
        itemDataPath = Application.dataPath + "/Resources/Json Files/ItemTest.json";
        playerInventoryItemPath = Application.dataPath + "/Resources/Json Files/PlayerInventoryItems.json";
        playerDataPath = Application.dataPath + "/Resources/Json Files/PlayerData.json";


        if (File.Exists(itemDataPath))
        {
            string itemJson = File.ReadAllText(itemDataPath);
            itemDataList = JsonUtility.FromJson<ItemDataList>(itemJson);
        }
        else
        {
            Debug.Log("ItemTest.json을 찾지 못 함");
            itemDataList = new ItemDataList();
        }

        if (File.Exists(playerInventoryItemPath))
        {
            string inventoryJson = File.ReadAllText(playerInventoryItemPath);
            inventoryList = JsonUtility.FromJson<InventoryItemList>(inventoryJson);
        }
        else
        {
            Debug.Log("PlayerInventoryItems.json을 찾지 못 함");
            inventoryList = new InventoryItemList();
        }

        if(File.Exists(playerDataPath))
        {
            string playerDataJson = File.ReadAllText(playerDataPath);
            playerData = JsonUtility.FromJson<GamePlayerData>(playerDataJson);
        }
        else
        {
            Debug.Log("PlayerData.json을 찾지 못 함");
            playerData = new GamePlayerData();
            playerData.items = new List<string>();
        }
    }

    private void AddInventory()
    {
        Item matchedItem = null;

        foreach (var item in itemDataList.Item)
        {
            if (itemAttribute.ItemID == item.itemID)
            {
                matchedItem = item;
                break;
            }
        }

        if (matchedItem != null)
        {
            ID = matchedItem.itemID;

            if (itemAttribute.ItemID == ID)
            {
                Description = matchedItem.itemDescription;

                ItemDescription newItem = new ItemDescription
                {
                    id = ID,
                    description = Description
                };

                inventoryList.itemDescription.Add(newItem);

                string updateInventoryJson = JsonUtility.ToJson(inventoryList, true);
                File.WriteAllText(playerInventoryItemPath, updateInventoryJson);
                Debug.Log("PlayerInventoryItems.json 파일 업데이트");

                AddPlayerData();
            }
        }
    }

    private void AddPlayerData()
    {
        string newItem = itemAttribute.ItemName;

        if(!playerData.items.Contains(newItem))
        {
            playerData.items.Add(newItem);

            // SaveData 호출..
            /*string updatePlayerDataJson = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(playerDataPath, updatePlayerDataJson);*/
            dataController.SaveData();
            Debug.Log("PlayerData.json 파일 업데이트");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        Debug.Log("플레이어랑 충돌!");
        ReadJsonFile();

#warning 나중에 플레이어 인벤토리 접근해서 그곳에 아이템 추가할 것
        AddInventory();
        Debug.Log($"아이템을 수집했습니다");

        Destroy(gameObject);
    }
}
