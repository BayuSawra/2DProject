using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy

{

    protected override void Awake()
    {
        base.Awake();
        patrolState = new BoarPatrolState(); //初始化巡逻状态赋值
        chaseState = new BoarChaseState(); //初始化追击状态赋值
    }


}
