using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface ISaveable
{
    DataDefination GetDataID();//一定要拿到ID的返回值
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this); //注册需要储存的数据的方法
    void UnRegisterSaveData() => DataManager.instance.UnRegisterSaveData(this);//注销数据

    void GetSaveData(Data data);//获取数据
    void LoadData(Data data);//读取数据
} 
