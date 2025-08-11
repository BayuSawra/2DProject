using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    [Header("广播")]
    public VoidEventSO saveGameEvent;

    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;
    public GameObject lightObj;
    public Sprite darkSprite;
    public Sprite lightSprite;
    public bool isDone;

    // private void Awake()
    // {
    //     spriteRenderer = GetComponent<SpriteRenderer>();
    // }
    void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;//已经储存了吗？没打开一个图，打开了一个图，用的三元运算符。
        lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        isDone = true;
        spriteRenderer.sprite = lightSprite;
        lightObj.SetActive(true);
        //TODU 保存数据
        saveGameEvent.RaiseEvent();

        this.gameObject.tag = "Untagged";
    }
}
