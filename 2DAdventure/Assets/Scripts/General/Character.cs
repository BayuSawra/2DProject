using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;

    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;

    [Header("受伤后无敌时间")]

    public float invulnerableDuration; // 受伤后的无敌时间

    [HideInInspector] public float invulnerableCounter; // 无敌计时器

    public bool invulnerable; // 是否无敌

    public UnityEvent<Character> OnHealthChange; // 生命值变化事件

    public UnityEvent<Transform> OnTakeDamage; // 受伤事件

    public UnityEvent OnDie; // 死亡事件

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void NewGame()
    {
        currentHealth = maxHealth; // 初始化当前生命值为最大生命值
        currentPower = maxPower;
        OnHealthChange?.Invoke(this); // 初始化时调用生命值变化事件，传入当前角色实例
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }


    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime; // 减少无敌计时器
            if (invulnerableCounter <= 0)
            {
                invulnerable = false; // 无敌时间结束
            }
        }

        if (currentPower < maxPower)
        {

            currentPower += Time.deltaTime * powerRecoverSpeed;

        }
    }

    private void OnTriggerStay2D(Collider2D other)//触发水面事件
    {
        if (other.CompareTag("Water"))
        {
            currentHealth = 0;
            OnHealthChange?.Invoke(this);
            OnDie?.Invoke();//死亡
        }
    }
    public void TakeDamage(Attack attacker)
    {

        if (invulnerable)
        {
            return; // 如果处于无敌状态，则不处理伤害
        }

        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable(); // 被打倒一次就触发无敌状态
            OnTakeDamage?.Invoke(attacker.transform);//询问确定有伤害事件订阅者，并调用事件，传入的坐标是对方的坐标。

        }

        else
        {
            currentHealth = 0; // 生命值归零
            OnDie?.Invoke();// 调用死亡方法
        }

        OnHealthChange?.Invoke(this); // 调用生命值变化事件，传入当前角色实例
    }

    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {

            invulnerable = true; // 设置为无敌状态
            invulnerableCounter = invulnerableDuration; // 重置无敌计时器
            // 可以在这里添加无敌状态的视觉效果或音效   
        }

    }
    
     public void OnSlide(int cost)//滑铲的消耗
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = transform.position;
            data.floatSaveData[GetDataID().ID + "power"] = this.currentPower;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            data.floatSaveData.Add(GetDataID().ID + "health", this.currentHealth);
            data.floatSaveData.Add(GetDataID().ID + "power", this.currentPower);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID];
            this.currentHealth = data.floatSaveData[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveData[GetDataID().ID + "power"];

            //通知UI更新
            OnHealthChange?.Invoke(this);
        }
    }
}
