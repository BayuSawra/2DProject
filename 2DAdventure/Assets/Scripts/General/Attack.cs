using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("攻击属性")]

    public int damage;//攻击力

    public float attackRange;//攻击范围

    public float attackRate;//攻击速率

    private void OnTriggerStay2D(Collider2D other)
    {
        other.GetComponent<Character>()?.TakeDamage(this);
    }
}
