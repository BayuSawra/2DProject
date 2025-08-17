using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public Transform Pos1;
    public Transform Pos2;
    private Transform MovePos;
    [SerializeField] private float MoveSpeed;
    void Start()
    {
        MovePos = Pos1;
    }

    
    void Update()
    {
        if (Vector2.Distance(transform.position, Pos1.position) < 0.1f)
        {
            MovePos = Pos2;
        }
        
        if (Vector2.Distance(transform.position, Pos2.position) < 0.1f)
        {
            MovePos = Pos1;
        }

        transform.position = Vector2.MoveTowards(transform.position, MovePos.position,MoveSpeed*Time .deltaTime);
    }
}
