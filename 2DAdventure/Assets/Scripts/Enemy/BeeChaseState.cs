using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeChaseState : BaseState
{
    private Attack attack;
    private Vector3 target;
    private Vector3 moveDir;
    private bool isAttack;
    private float attackRangeCounter = 0;

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attack = enemy.GetComponent<Attack>();

        currentEnemy.lostTimeCounter = currentEnemy.lostTime; //重置丢失时间计时器

        currentEnemy.anim.SetBool("chase", true); //设置动画状态为追击
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol); //如果丢失时间计时器小于等于0，切换到巡逻状态
        }

         //计时器
        attackRangeCounter -= Time.deltaTime;

        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0);//1.5是因为蜜蜂和玩家的中心点不一样，玩家的中心点在脚底，所以在Y轴上要向上加数值

        //判断攻击距离
        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) <= attack.attackRange && Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attack.attackRange)
        {
            //攻击
            isAttack = true;//进入攻击状态
            if (!currentEnemy.isHurt)
            {
                currentEnemy.rb.velocity = Vector2.zero; //如果在攻击范围内，则将刚体的速度设置为0
            }

            if (attackRangeCounter <= 0)
            {
                attackRangeCounter = attack.attackRate; //重置攻击计时器
                currentEnemy.anim.SetTrigger("attack"); //触发攻击动画

            }
        }
        else //超出攻击范围
        {
            isAttack = false; //不在攻击状态
        }
        
        moveDir = (target - currentEnemy.transform.position).normalized; //计算移动方向

        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1); //如果移动方向的x轴分量大于0，则将物体的局部缩放的x轴方向设置为1
        if (moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1); //如果移动方向的x轴分量小于0，则将物体的局部缩放的x轴方向设置为-1


    }

    public override void PhysicsUpdate()
    {
          if ( !currentEnemy.isHurt && !currentEnemy.isDead && !isAttack) //如果没有等待且没有受伤且没有死亡没有在攻击
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("chase", false); //退出追击状态时，设置动画状态为非追击
    }
}
