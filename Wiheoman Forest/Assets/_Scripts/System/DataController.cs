using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public bool isDead;
    public string name;
    public int level;
    public int coin;
    public string[] itmes;
}

public class DataController : MonoBehaviour
{
    public static DataController instance;
    PlayerData player = new PlayerData();

    private string path;
    private string fileName = "Save";


    private void Awake()
    {
        #region Singleton
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        #endregion

        path = Application.persistentDataPath + "/";
    }

    private void Start()
    {

    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(player, true);
        File.WriteAllText(path + fileName, data);
        Debug.Log(path + fileName);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + fileName);
        JsonUtility.FromJson<PlayerData>(data);
    }
}