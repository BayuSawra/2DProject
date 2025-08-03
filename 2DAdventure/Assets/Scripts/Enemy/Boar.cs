using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy

{
    public override void Move()
    {
        base.Move();
        anim.SetBool("walk", true); //设置动画参数，控制是否走动
    }  






}
