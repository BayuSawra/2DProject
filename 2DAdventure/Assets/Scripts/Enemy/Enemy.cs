using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    protected Animator anim;

    PhysicsCheck physicsCheck;

    [Header("基本参数")]
    public float normalSpeed; //走动速度
    public float chaseSpeed; //追击速度
    public float currentSpeed; //目前速度范围
    public Vector3 faceDir;//三维的向量，记录面朝的方向

    [Header("计时器")]
    public float waitTime; //等待时间

    public float waitTimeCounter; //等待时间计时器

    public bool wait; //是否等待

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //获取当前物体的Rigidbody2D组件
        anim = GetComponent<Animator>(); //获取当前物体的Animator组件
        physicsCheck = GetComponent<PhysicsCheck>(); //获取当前物体的PhysicsCheck组件
        currentSpeed = normalSpeed; //初始化当前速度为正常速度

        waitTimeCounter= waitTime; //初始化等待时间计时器为等待时间

    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0); //面朝方向为当前物体的局部缩放的x轴方向，y轴和z轴为0

        if ((physicsCheck.touchLeftWall && faceDir.x < 0) || (physicsCheck.touchRightWall && faceDir.x > 0))//如果接触左墙壁或右墙壁
        {
            wait = true; //设置等待为true
            anim.SetBool  ("walk", false); //设置动画参数，控制是否走动
        }

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
        Move(); //每帧调用Move方法
    }

    public virtual void Move()
    {

        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y); //设置刚体的速度为当前速度乘以面朝方向的x轴分量，y轴速度保持不变

    }

    public void TimeCounter()
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



    }
}
