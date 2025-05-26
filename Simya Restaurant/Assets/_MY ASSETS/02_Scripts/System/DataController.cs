using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

[System.Serializable]
public class PlayerItemSingle
{
    public int id;
    public int count;
}

[System.Serializable]
public class PlayerData
{
    public static PlayerData instance;

    [Header("Status")]
    public bool isDead;
    public string name;
    public int level;
    public int gold;
    public int potionsRemain;
    public float health;
    public float cooltimePotion; // 포션 사용을 허용하는 시간을 저장할 수 있으나, 저장 및 로드에서 복잡해짐.

    [Space(30)]
    [Header("Inventory")]
#warning items는 더이상 쓰지 않음 itemsData을 사용허세요
    public string[] items;
    public PlayerItemSingle[] itemsData;

    [Space(30)]
    [Header("Recipes")]
    public string[] recepies;
    public bool quests;

    public PlayerData()
    {
        items = new string[0];
        instance = this;
    }

    /// <summary>
    /// Candy, CelebrityAutographs, MagicPowder가 있는지 체크
    /// </summary>
    /// <returns></returns>
    public bool HasSpecialItem()
    {
        return items.Contains("Candy") || items.Contains("CelebrityAutographs") || items.Contains("MagicPowder");
    }
}


public class DataController : MonoBehaviour
{
    static public DataController instance;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private bool isDebugging;
    private List<PlayerItemSingle> playerItemList = new List<PlayerItemSingle>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
        playerItemList = playerData.itemsData.ToList();
    }

    public void MakeNew()
    {
        playerData = new PlayerData()
        {
            isDead = false,
            name = "NoName",
            level = 1,
            gold = 500,
            potionsRemain = 3,
            health = 100.0f,
            cooltimePotion = 0.0f,
            itemsData = new PlayerItemSingle[0] { },
            recepies = new string[0] { },
            quests = false
        };
    }

    public void SaveData()
    {
        playerData.itemsData = playerItemList.ToArray();
        string json = JsonUtility.ToJson(playerData, true);
        string path = Application.dataPath + "/Resources/Json Files/PlayerData.json";
        File.WriteAllText(path, json);
        Debug.Log("Save");
    }

    public void LoadData()
    {
        string path = Application.dataPath + "/_MY ASSETS/03_Resources/Json Files/PlayerData.json";

        if (isDebugging)
        {
            Console.WriteLine($"LoadData() : File.Exists(path) == {File.Exists(path)}");
        }

        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(data);
            playerItemList = playerData.itemsData.ToList();
        }
        else
        {
            Debug.LogError("Not Found 'PlayerData.json' File.");
        }
        Debug.Log("Load");
    }

    public PlayerData Access()
    {
        return playerData;
    }

    public bool IsExist(int id, int amount = 1)
    {
        int index = M_GetIndex(id);
        if (index == -1)
        {
            return false;
        }
        PlayerItemSingle one = playerItemList[index];
        if (one.count < amount)
        {
            return false;
        }
        return true;
    }

    public int M_GetIndex(int id)
    {
        for (int index = 0; index < playerItemList.Count; ++index)
        {
            PlayerItemSingle one = playerItemList[index];
            if (isDebugging)
            {
                Debug.Log($"M_GetIndex(int id) one : {one.id} search {id}, {one.id != id} ");
            }
            if (one.id != id) continue;
            return index;
        }
        return -1;
    }

    public void AddItem(int id, int amount = 1)
    {
        if (amount == 0)
        {
            return;
        }

        int index = M_GetIndex(id);
        if (index == -1)
        {
            Debug.Log("아이템 아이디 찾을 수 없음");
            playerItemList.Add(new PlayerItemSingle() { id = id, count = amount });
            foreach (PlayerItemSingle s in playerItemList)
            {
                Debug.Log($"dataController playerItemList.Count : {s.id} / {s.count}");
            }
        }
        else
        {
            playerItemList[index].count += amount;
        }
    }

    public bool TryRemoveItem(int id, int amount = 1)
    {
        int index = M_GetIndex(id);
        if (index == -1)
        {
            return false;
        }
        else if (playerItemList[index].count < amount)
        {
            return false;
        }
        else if (playerItemList[index].count == amount)
        {
            playerItemList.RemoveAt(index);
        }
        else
        {
            playerItemList[index].count -= amount;
        }
        return true;
    }
    public (bool isExist, int index) HasRecepie(string recepieConst)
    {
        for (int index = 0; index < playerData.recepies.Length; ++index)
        {
            if (recepieConst.Equals(playerData.recepies[index]))
            {
                return (true, index);
            }
        }
        return (false, -1);
    }
    public void AddRecepie(string recepieConst)
    {
        if (HasRecepie(recepieConst).isExist)
        {
            return;
        }
        List<string> m_list = playerData.recepies.ToList();
        m_list.Add(recepieConst);
        playerData.recepies = m_list.ToArray();
    }
    public bool TryRemoveRecipe(string recepieConst)
    {
        int index = HasRecepie(recepieConst).index;
        if (index == -1)
        {
            return false;
        }
        List<string> m_list = playerData.recepies.ToList();
        m_list.RemoveAt(index);
        playerData.recepies = m_list.ToArray();
        return true;
    }
}