using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ItemData
{
    public List<Item> ItemDatas = new List<Item>();
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [HideInInspector] public Dictionary<string, Item> ItemDataDic = new Dictionary<string, Item>();

    string ITEMDATA_PATH = Application.dataPath + "/Resources/Data/ItemDatas.json";

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }
    private void LoadData()
    {
        if (!File.Exists(ITEMDATA_PATH))
        {
            Debug.LogWarning("No Item Datas");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            var json = File.ReadAllText(ITEMDATA_PATH);
            var datas = JsonUtility.FromJson<ItemData>(json);
            foreach(var data in  datas.ItemDatas)
            {
                ItemDataDic.Add(data.Rcode, data);
            }
        }
    }

    public Item GetData(string rcode)
    {
        return ItemDataDic[rcode];
    }
}
