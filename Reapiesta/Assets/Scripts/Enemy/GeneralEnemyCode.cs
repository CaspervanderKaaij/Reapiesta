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
    public float targetDistCheck;
    public Animator anim;
    public bool trigger;
    public Talk talk;

    public virtual void Start()
    {
        talk = GetComponent<Talk>();
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
            targetDistCheck = Vector3.Distance(transform.position, target); 
            if (targetDistCheck <= minDist && state == MoveState.chasing)
            {
                action = true;
                // when your distance is close enough stop 
                GetComponent<Ground>().groundAgent.isStopped = true;
                // change state to attcking
                GetComponent<Ground>().attacking = true;
            }
            if (targetDistCheck > minDist)
            {
                PlaySound(2,1);
                GetComponent<Ground>().groundAgent.isStopped = false;
                GetComponent<Ground>().moveState = MoveState.chasing;
            }
            else
            {
                PlaySound(2,1);
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
                PlaySound(0,2);
                anim.SetTrigger("Attack");
                Invoke("Action",0.55f);
                currentTime = time;
            }
        }
    }
    public virtual void PlaySound(int index,float priority)
    {   
        GetComponent<Talk>().Speak(index,priority);
    }
    public virtual void Action()
    {
    }
}
