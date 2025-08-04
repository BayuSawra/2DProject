using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        patrolState = new SnailPatrolState();
        skillState = new SnailSkillState();
    }

    // 重写父类的OnHurt协程
    protected override IEnumerator OnHurt(Vector2 dir)
    {
        // 蜗牛的特殊受伤效果：缩进壳里
        anim.SetTrigger("hide");
        
        // 调用父类的受伤逻辑
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        
        // 蜗牛受伤时间更长
        yield return new WaitForSeconds(1.0f);
        
        // 从壳里出来
        anim.SetTrigger("show");
        yield return new WaitForSeconds(0.3f);
        
        isHurt = false;
    }
}
