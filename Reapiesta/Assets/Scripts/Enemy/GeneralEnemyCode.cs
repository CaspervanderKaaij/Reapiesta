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
    public bool trigger;

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
        if (trigger)
        {
            targetDist = Vector3.Distance(transform.position, target);
            if (targetDist <= minDist && state == MoveState.chasing)
            {
                print("Trigger");
                action = true;
                // when your distance is close enough stop 
                GetComponent<Ground>().groundAgent.isStopped = true;
                // change state to attcking
                GetComponent<Ground>().attacking = true;
            }
            if (minDist > targetDist)
            {
                PlaySound();
                GetComponent<Ground>().groundAgent.isStopped = false;
                GetComponent<Ground>().moveState = MoveState.walking;
            }
            else
            {
                PlaySound();
                GetComponent<Ground>().groundAgent.isStopped = true;
                GetComponent<Ground>().moveState = MoveState.idle;
            }
        }

    }
    public virtual void Timer(float time)
    {
        if (action)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                PlaySound();
                anim.SetTrigger("Attack");
                Invoke("Action",0.55f);
                currentTime = time;
            }
        }
    }
    public virtual void PlaySound()
    {   

    }
    public virtual void Action()
    {
    }
}
