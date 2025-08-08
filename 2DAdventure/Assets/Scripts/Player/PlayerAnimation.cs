using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;

    private void Awake()
    {
        anim = GetComponent<Animator>(); //获取当前物体的Animator组件
        rb = GetComponent<Rigidbody2D>(); //获取当前物体的Rigidbody2D组件
        physicsCheck = GetComponent<PhysicsCheck>(); //获取当前物体的PhysicsCheck组件
        playerController = GetComponent<PlayerController>(); //获取当前物体的PlayerController组件
    }

    private void Update()
    {
        SetAnimation();//每帧调用SetAnimation方法
    }

    public void SetAnimation()
    {
        anim.SetFloat("velocityX", Mathf.Abs(rb.velocity.x)); //设置动画参数，控制水平速度
        anim.SetFloat("velocityY", rb.velocity.y); //设置动画参数，控制垂直速度
        anim.SetBool("isGround", physicsCheck.isGround); //设置动画参数，控制是否在地面上
        anim.SetBool("isCrouch", playerController.isCrouch); //设置动画参数，控制是否蹲下
        anim.SetBool("isDead", playerController.isDead); //设置动画参数，控制是否死亡
        anim.SetBool("isAttack", playerController.isAttack); //设置动画参数，控制是否攻击
        anim.SetBool("onWall", physicsCheck.onWall);//设置动画，在墙面滑动。
        anim.SetBool("isSlide", playerController.isSlide);//设置动画，开始滑铲
    }

    public void PlayHurt()
    {

        anim.SetTrigger("hurt"); //触发受伤动画

    }
    
    public void PlayerAttack()
    {
        anim.SetTrigger("attack"); //触发攻击动画
    }

}
