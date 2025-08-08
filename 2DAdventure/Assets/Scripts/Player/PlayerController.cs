using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;//创建一个 PlayerInputControl类型的变量

    public Vector2 inputDirection; //存储移动输入的变量，用于储存人物移动的数据

    public Rigidbody2D rb; //引用Rigidbody2D组件，用于物理移动

    private CapsuleCollider2D coll;//引用CapsuleCollider2D组件，用于控制碰撞体大小

    private PhysicsCheck physicsCheck; //引用PhysicsCheck脚本，用于检测地面

    private PlayerAnimation playerAnimation; //引用PlayerAnimation脚本，用于控制动画

    [Header("基本参数")]
    public float speed;//声明移动速度变量

    private float runSpeed;
    private float walkSpeed => speed / 2.8f; //声明行走速度变量，等于移动速度的一半
    private Vector2 originalOffset; //原始碰撞体偏移量
    private Vector2 originalSize; //原始碰撞体大小

    [Header("物理材质")]
    public PhysicsMaterial2D normal; //有摩擦力的物理材质
    public PhysicsMaterial2D wall; //无摩擦力的物理材质

    [Header("状态")]
    public float jumpForce; //声明跳跃力变量

    public bool isCrouch;//是否蹲下

    public float hutForce; //受伤时的冲力
    public bool isHurt; //是否受伤

    public bool isDead; //是否死亡

    public bool isAttack; //是否攻击




    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //获取当前物体的Rigidbody2D组件

        physicsCheck = GetComponent<PhysicsCheck>(); //获取当前物体的PhysicsCheck组件

        coll = GetComponent<CapsuleCollider2D>(); //获取当前物体的CapsuleCollider2D组件

        playerAnimation = GetComponent<PlayerAnimation>(); //获取当前物体的PlayerAnimation组件

        originalOffset = coll.offset; //保存原始碰撞体偏移量
        originalSize = coll.size; //保存原始碰撞体大小

        inputControl = new PlayerInputControl(); //实例化 PlayerInputControl


        inputControl.Gameplay.Jump.started += Jump; //注册(+=)跳跃事件，当按下跳跃键时调用Jump方法.把Jump方法绑定到Jump事件上,在按键按下的那一刻执行。

        #region  按住shift强制走路
        runSpeed = speed; //初始化跑步速度为移动速度

        inputControl.Gameplay.WalkButton.performed += ctx =>
        {
            if (physicsCheck.isGround) //如果在地面上
                speed = walkSpeed; //设置速度为行走速度

        };
        inputControl.Gameplay.WalkButton.canceled += ctx =>
        {
            if (physicsCheck.isGround) //如果在地面上
                speed = runSpeed; //设置速度为跑步速度
        };
        #endregion

        inputControl.Gameplay.Attack.started += PlayerAttack; //注册攻击事件，当按下攻击键时调用PlayerAttack方法

    }


    private void OnEnable()
    {
        inputControl.Enable(); //启用 PlayerInputControl
    }

    private void OnDisable()
    {
        inputControl.Disable(); //禁用 PlayerInputControl
    }

    private void Update() //，每帧执行
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>(); //获取玩家的移动输入
        CheckState(); //检查状态
    }

    private void FixedUpdate()  //固定执行
    {
        if (!isHurt && !isAttack)//如果没有受伤
            Move(); //调用移动方法
    }

    public void Move()
    {
        if (!isCrouch) //如果没有下蹲，才能移动
        {
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y); //根据输入方向和速度设置刚体的速度,y轴保持真实物理速度-9.81 
        }

        int faceDir = (int)transform.localScale.x;

        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;

        //人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);


        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround; //判断是否蹲下，条件为y轴输入小于-0.5且在地面上
        //控制碰撞体大小
        if (isCrouch)
        {
            coll.offset = new Vector2(-0.05f, 0.85f); //设置碰撞体偏移量
            coll.size = new Vector2(0.7f, 1.7f); //设置碰撞体大小
        }
        else
        {
            coll.offset = originalOffset; //恢复原始碰撞体偏移量
            coll.size = originalSize; //恢复原始碰撞体大小
        }
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        //Debug.Log("Jump"); //打印跳跃信息
        if (physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); //添加向上的冲力，实现跳跃
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        // if(!physicsCheck.isGround)
        //     return; //如果不在地面上，直接返回，不执行攻击
        playerAnimation.PlayerAttack(); //调用PlayerAnimation脚本中的PlayerAttack方法，触发攻击动画
        isAttack = true; //设置攻击状态为true
    }


    #region unity事件
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero; //清除当前速度
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized; //计算受伤方向

        rb.AddForce(dir * hutForce, ForceMode2D.Impulse); //添加受伤冲力

    }

    public void PlayerDead()
    {

        isDead = true;
        inputControl.Gameplay.Disable(); //禁用手柄输入
        CheckState();//禁止死后被攻击

    }

    #endregion
    
       private void CheckState()//检查状态
    {   
        coll.sharedMaterial = physicsCheck.isGround? normal : wall; //根据是否在地面上设置碰撞体的物理材质,三元运算符

         if (physicsCheck.onWall)//在墙面时下降速度是原来的一半
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y *0.95f);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);




        if (isDead)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");
    }
    
}
