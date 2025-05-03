using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// =========================================================
// ������ ���� Json ���� 
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
// �÷��̾� �κ��丮 ���� Json ���� 
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
// �÷��̾� ���� Json ���� 
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
            Debug.Log("ItemTest.json�� ã�� �� ��");
            itemDataList = new ItemDataList();
        }

        if (File.Exists(playerInventoryItemPath))
        {
            string inventoryJson = File.ReadAllText(playerInventoryItemPath);
            inventoryList = JsonUtility.FromJson<InventoryItemList>(inventoryJson);
        }
        else
        {
            Debug.Log("PlayerInventoryItems.json�� ã�� �� ��");
            inventoryList = new InventoryItemList();
        }

        if(File.Exists(playerDataPath))
        {
            string playerDataJson = File.ReadAllText(playerDataPath);
            playerData = JsonUtility.FromJson<GamePlayerData>(playerDataJson);
        }
        else
        {
            Debug.Log("PlayerData.json�� ã�� �� ��");
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
                Debug.Log("PlayerInventoryItems.json ���� ������Ʈ");

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

            // SaveData ȣ��..
            /*string updatePlayerDataJson = JsonUtility.ToJson(playerData, true);
            File.WriteAllText(playerDataPath, updatePlayerDataJson);*/
            dataController.SaveData();
            Debug.Log("PlayerData.json ���� ������Ʈ");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        AudioManager.instance.PlaySfx(AudioManager.SFX.SkipConversation);

        Debug.Log("�÷��̾�� �浹!");
        ReadJsonFile();

#warning ���߿� �÷��̾� �κ��丮 �����ؼ� �װ��� ������ �߰��� ��
        AddInventory();
        Debug.Log($"�������� �����߽��ϴ�");

        Destroy(gameObject);
    }
}
