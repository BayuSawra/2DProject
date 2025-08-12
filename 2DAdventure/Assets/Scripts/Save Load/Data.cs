using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    //字典
    public string sceneToSave;
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();//字典里需要传入一个string和vector3的变量

    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    public void SaveGameScene(GameSceneSO savedScene)
    {
        sceneToSave = JsonUtility.ToJson(savedScene);//将object类型的内容转换成json，传入序列化之后的string文件。
        Debug.Log("传进来的是" + sceneToSave);
    }

    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);//将json文件反序列化出来变为c语言文件，获得之保存的内容

        return newScene;
    }
}
