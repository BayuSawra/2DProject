using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(PhysicsCheck))]

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    [HideInInspector] public Animator anim;

    [HideInInspector] public PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed; //走动速度
    public float chaseSpeed; //追击速度
    [HideInInspector] public float currentSpeed; //目前速度范围
    public Vector3 faceDir;//三维的向量，记录面朝的方向
    public float hurtForce; //受伤时的冲力
    public Transform attacker; //攻击者的Transform，用于记录攻击者的位置

    [Header("检测参数")]
    public Vector2 centerOffset; //检测中心偏移
    public Vector2 checkSize; //检测区域大小
    public float checkDistance; //检测距离
    public LayerMask attackLayer; //攻击层

    [Header("计时器")]
    public float waitTime; //等待时间

    public float waitTimeCounter; //等待时间计时器

    public bool wait; //是否等待

    public float lostTime; //丢失时间
    public float lostTimeCounter; //丢失时间计时器

    [Header("状态")]
    public bool isHurt; //是否受伤

    public bool isDead; //是否死亡

    private BaseState currentState; //当前状态

    protected BaseState patrolState; //巡逻状态

    protected BaseState chaseState; //追击状态

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //获取当前物体的Rigidbody2D组件
        anim = GetComponent<Animator>(); //获取当前物体的Animator组件
        physicsCheck = GetComponent<PhysicsCheck>(); //获取当前物体的PhysicsCheck组件
        currentSpeed = normalSpeed; //初始化当前速度为正常速度

        waitTimeCounter = waitTime; //初始化等待时间计时器为等待时间

    }
    private void OnEnable()

    {
        currentState = patrolState; //将当前状态设置为巡逻状态
        currentState.OnEnter(this); //调用当前状态的OnEnter方法
    }
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0); //面朝方向为当前物体的局部缩放的x轴方向，y轴和z轴为0

        currentState.LogicUpdate(); //调用当前状态的逻辑更新方法
        TimeCounter();

        //下方是网网友写的
        // if (physicsCheck.touchLeftWall)
        // {
        //     transform.localScale = new Vector3(-1, 1, 1); //如果接触左墙壁，则将物体的局部缩放的x轴方向设置为-1，表示面朝右侧
        // }
        // else if (physicsCheck.touchRightWall)
        // {
        //     transform.localScale = new Vector3(1, 1, 1); //如果接触右墙壁，则将物体的局部缩放的x轴方向设置为1，表示面朝左侧
        // }

    }

    private void FixedUpdate()
    {
        currentState.PhysicsUpdate(); //调用当前状态的物理更新方法     
       if (!isHurt && !isDead && !wait) //如果没有受伤且没有死亡
            Move(); //每帧调用Move方法

    }

    private void OnDisable()
    {
        currentState.OnExit(); //调用当前状态的OnExit方法
    }

    public virtual void Move()
    {

        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y); //设置刚体的速度为当前速度乘以面朝方向的x轴分量，y轴速度保持不变

    }

    public void TimeCounter()//计时器
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime; //如果等待，则计时器减去时间增量
            if (waitTimeCounter <= 0)
            {
                wait = false; //如果计时器小于等于0，则不再等待
                waitTimeCounter = waitTime; //重置计时器为等待时间
                transform.localScale = new Vector3(faceDir.x, 1, 1); //将物体的局部缩放的x轴方向取反，表示面朝相反方向
            }

        }

        if (!FoundPlayer() && lostTimeCounter > 0) //如果没有找到玩家且丢失时间计时器大于0
        {

            lostTimeCounter -= Time.deltaTime; //如果没有找到玩家，则丢失时间计时器减去时间增量
        }

        else if (FoundPlayer())
        {

            lostTimeCounter = lostTime; //如果找到了玩家，则将丢失时间计时器重置为丢失时间

        }
        
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);

    }

    public void SwitchState(NPCState state)//切换状态,语法糖没学过，以后看看
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => currentState
        };

        currentState.OnExit(); //调用当前状态的OnExit方法
        currentState = newState; //将当前状态设置为新的状态
        currentState.OnEnter(this); //调用新的状态的OnEnter方法，并传入当前状态

    }

    #region  事件执行方法

    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        //转身
        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        //受伤被击退
        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0,rb.velocity.y); //重置速度
        StartCoroutine(OnHurt(dir));//调用onhurt协程
    }

    private IEnumerator OnHurt(Vector2 dir) //被打后暂停一下
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f); //等待0.45秒
        isHurt = false; //受伤状态结束
    }

    public void OnDie()
    {
        gameObject.layer = 8; //将当前物体的层级设置为2(ignorRaycest什么的那个)，通常表示死亡状态
        anim.SetBool("dead", true); //设置动画参数，控制死亡动画
        isDead = true; //设置死亡状态为true

    }

    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject); //销毁当前物体
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //设置Gizmos颜色为红色
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+ new Vector3(checkDistance*-transform.localScale.x,0), 0.2f);
    }
}
