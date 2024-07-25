using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour
{
    public PlayerData playerData;

    [ContextMenu("To Json Data")]
    void SaveDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Application.dataPath + "/Resources/Json Files/PlayerData.json";
        File.WriteAllText(path, jsonData);
    }

    [ContextMenu("From Json Data")]
    void LoadDataFromJson()
    {
        string path = Application.dataPath + "/Resources/Json Files/PlayerData.json";
        string jsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }
}

[System.Serializable]
public class PlayerData
{
    public bool isDead;
    public string name;
    public int level;
    public int gold;
    public string[] items;  
}