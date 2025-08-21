using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitalLoad : MonoBehaviour
{
    public AssetReference persistentScene;

    void Awake()
    {
        Addressables.LoadSceneAsync(persistentScene);
    }
}
