using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
// Start is called before the first frame update
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed; //设置巡逻速度
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())//如果发现了玩家
        { 
            currentEnemy.SwitchState(NPCState.Chase); //切换到追击状态
        }
        

        if (!currentEnemy.physicsCheck.isGround ||
             (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) ||
             (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))//如果接触左墙壁或右墙壁
            {
                currentEnemy.wait = true; //设置等待为true
                currentEnemy.anim.SetBool("walk", false); //设置动画参数，控制是否走动
            }

            else
            {

                currentEnemy.anim.SetBool("walk", true); //设置动画参数，控制是否走动

            }

    }

    public override void PhysicsUpdate()
    {
        

    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("walk", false); //退出时停止走动
        Debug.Log("巡逻结束");
        
    }
}

