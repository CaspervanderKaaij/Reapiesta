using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class GeneralEnemyCode : MonoBehaviour
{
    public float currentTime;
    public bool action;
    public float targetDist;
    public Animator anim;

    public virtual void Start()
    {
        //empty
    }

    public virtual void Update()
    {
        //empty
    }

    public virtual void CheckDist(Vector3 target, float minDist, MoveState state)
    {
        // check the distance of your target
        targetDist = Vector3.Distance(transform.position, target);
        if (targetDist < minDist && state == MoveState.chasing)
        {
            action = true;
            // when your distance is close enough stop 
            GetComponent<Ground>().groundAgent.isStopped = true;
            // change state to attcking
            GetComponent<Ground>().moveState = MoveState.attacking;
        }
        else
        {
            action = false;
            GetComponent<Ground>().groundAgent.isStopped = false;
        }
    }
    public virtual void Timer(float time)
    {
        if (action)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = time;
                Action();
            }
        }
    }
    public virtual void Action()
    {
    }
}
