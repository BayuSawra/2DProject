using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;
using System.IO;

//设计模式：观察者模式/广播模式
[DefaultExecutionOrder(-100)]//数字越小，越先执行
public class DataManager : MonoBehaviour
{
    public static DataManager instance;//单例模式

    [Header("事件监听")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    private List<ISaveable> saveableList = new List<ISaveable>();//创建储存列表
    private Data saveData;
    private string jsonFloder;
    public bool HasSave { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        saveData = new Data();

        jsonFloder = Application.persistentDataPath + "/SAVE DATA/";//自动定位不同处理器的储存位置
        ReadSaveData();
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        var resultPath = jsonFloder + "data.sav";
        var jsonData = JsonConvert.SerializeObject(saveData);
        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFloder);
        }

        File.WriteAllText(resultPath, jsonData);//写入data.sav
        HasSave = true;

        // foreach (var item in saveData.characterPosDict)//看看存没存对
        // {
        //     Debug.Log(item.Key + "     " + item.Value);
        // }
    }

    public void Load()
    {
        if (!HasSave)
        {
            Debug.LogWarning("No save data, skip Load()");
            return;
        }
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }

    private void ReadSaveData()
    {
        var resultPath = jsonFloder + "data.sav";
        HasSave = File.Exists(resultPath);
        if (HasSave)
        {
            var stringData = File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);
            saveData = jsonData;
        }
        
    }
}
