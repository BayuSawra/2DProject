using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public PersistentType persistentType;
    public string ID;

    private void OnValidate()
    {
        if (persistentType == PersistentType.ReadWrite)
        {
            if (ID == string.Empty)
            {
                ID = System.Guid.NewGuid().ToString();//自动生成全局唯一标识，每个都不同
            }
        }
        else
        {
            ID = string.Empty;
        }
       
    }
}
