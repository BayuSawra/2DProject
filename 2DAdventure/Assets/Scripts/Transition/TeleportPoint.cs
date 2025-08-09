using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public void TriggerAction()
    {
        Debug.Log("传送！");

    }
}
