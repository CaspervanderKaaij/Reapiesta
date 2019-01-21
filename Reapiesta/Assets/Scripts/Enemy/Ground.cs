using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ground : MonoBehaviour
{
    Animator anim;
    public MoveState moveState;
    public Vector3 target;
    public GroundStats groundStats;
    public NavMeshAgent groundAgent;
    float mintargetDist;
    void Start()
    {
        groundAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        mintargetDist = groundStats.mintargetDist;
        moveState = MoveState.walking;
        if(GetComponent<Range>() != null)
        {
            anim.SetBool("Trow", true);
        }
        else
        {
            anim.SetBool("Attack", true);
        }
    }
    void Update()
    {
        Check();
        CheckDistance();
    }

    public void Check()
    {
        switch (moveState)
        {
            case MoveState.idle:
                // set the currentMovement to idle speed
                Idle(target);
                break;
            case MoveState.walking:
                Walking();
                groundAgent.speed = groundStats.walkSpeed;
                // set the currentMovement speed to walking speed
                break;
            case MoveState.chasing:
                Walking();
                groundAgent.speed = groundStats.runSpeed;
                //set currentMovement speed to running speed
                break;
            case MoveState.attacking:
            Vector3 newpos = transform.position;
                Idle(newpos);
                break;
        }
    }
    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, target) < mintargetDist)
        {
            moveState = MoveState.idle;
        }
    }
    void Idle(Vector3 newTarget)
    {
        groundAgent.SetDestination(newTarget);
        //set a enemy to a position
        //play an animation
        anim.SetFloat("Smooth", Mathf.Lerp(anim.GetFloat("Smooth"),0,Time.deltaTime * 2));
    }
    void Walking()
    {
        groundAgent.SetDestination(target);
        // walk random around the area
        anim.SetFloat("Smooth", Mathf.Lerp(anim.GetFloat("Smooth"),0.5f,Time.deltaTime * 2));
    }
}
