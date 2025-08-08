using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("追击中");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed; //设置追击速度
        currentEnemy.anim.SetBool("run", true);
    }
   
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        { 
           currentEnemy.SwitchState(NPCState.Patrol); //如果丢失时间计时器小于等于0，切换到巡逻状态
        }
        

        if (!currentEnemy.physicsCheck.isGround ||
             (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) ||
             (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))//如果接触左墙壁或右墙壁
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1); //翻转物体的缩放方向
        }
    }

    public override void PhysicsUpdate()
    {
        
    }
   
    public override void OnExit()
    {
        
        currentEnemy.anim.SetBool("run", false); //退出时停止奔跑
    }
}
