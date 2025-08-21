//抽象基类
using UnityEngine;
using UnityEditor;

public abstract class BaseState
{
    protected Enemy currentEnemy; //当前敌人
    public abstract void OnEnter(Enemy enemy);//进入状态时调用，传入当前敌人

    public abstract void LogicUpdate();//判断类比如bool

    public abstract void PhysicsUpdate();//物理更新

    public abstract void OnExit();



}