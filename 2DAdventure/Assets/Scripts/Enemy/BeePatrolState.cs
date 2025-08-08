using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeePatrolState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
        target = enemy.GetNewPoint();//获取新的目标点
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }

        if (Mathf.Abs(target.x - currentEnemy.transform.position.x) < 0.1f && Mathf.Abs(target.y - currentEnemy.transform.position.y) < 0.1f)
        {
            currentEnemy.wait = true; //如果到达目标点，则设置等待状态为true
            target = currentEnemy.GetNewPoint(); //如果到达目标点，则获取新的目标点
        }

        moveDir = (target - currentEnemy.transform.position).normalized; //计算移动方向

        if (moveDir.x > 0)
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1); //如果移动方向的x轴分量大于0，则将物体的局部缩放的x轴方向设置为1
        if (moveDir.x < 0)
            currentEnemy.transform.localScale = new Vector3(1, 1, 1); //如果移动方向的x轴分量小于0，则将物体的局部缩放的x轴方向设置为-1
    }

    public override void PhysicsUpdate()
    {
        if (!currentEnemy.wait && !currentEnemy.isHurt && !currentEnemy.isDead) //如果没有等待且没有受伤且没有死亡
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }

        else
        { 
            currentEnemy.rb.velocity = Vector2.zero; //如果等待，则将刚体的速度设置为0
        }
    }   

    public override void OnExit()
    {
        
    }
 }
