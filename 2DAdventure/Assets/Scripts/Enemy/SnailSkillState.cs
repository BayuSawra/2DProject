
using UnityEngine;

public class SnailSkillState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed; //设置追击速度
        currentEnemy.anim.SetBool("walk", false); //设置动画参数，控制是否走动
        currentEnemy.anim.SetBool("hide", true); //设置受伤动画参数为false
        currentEnemy.anim.SetTrigger("skill"); //触发技能动画

        currentEnemy.lostTimeCounter = currentEnemy.lostTime; //重置丢失时间计时器

        currentEnemy.GetComponent<Character>().invulnerable = true; //设置无敌状态为true，防止在技能状态下受到伤害
        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;//将无敌的时间设置为发现玩家后的losttime
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {

            currentEnemy.SwitchState(NPCState.Patrol); //如果丢失时间计时器小于等于0，切换到巡逻状态

        }

        currentEnemy.GetComponent<Character>().invulnerableCounter = currentEnemy.lostTimeCounter;//这里持续执行
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("hide", false); //退出时停止隐藏
        currentEnemy.GetComponent<Character>().invulnerable = false; //设置无敌状态为false，允许受到伤害

    }
}
    

