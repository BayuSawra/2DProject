using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤后无敌时间")]

    public float invulnerableDuration; // 受伤后的无敌时间

    [HideInInspector]public float invulnerableCounter; // 无敌计时器

    public bool invulnerable; // 是否无敌

    public UnityEvent<Transform> OnTakeDamage; // 受伤事件

    public UnityEvent OnDie; // 死亡事件


    private void Start()
    {
        currentHealth = maxHealth; // 初始化当前生命值为最大生命值
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
}
