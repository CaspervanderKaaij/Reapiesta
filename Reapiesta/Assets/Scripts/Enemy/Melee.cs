using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : GeneralEnemyCode
{
    public MeleeStats meleeStats;
    public Vector3 target;
    public GameObject hurtBox;


    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        hurtBox = transform.GetChild(1).gameObject;
        hurtBox.SetActive(false);
        targetDist = meleeStats.mintargetDist;
        currentTime = meleeStats.attackSpeed;
    }

    public override void Update()
    {
        if (trigger)
        {
            Timer(meleeStats.attackSpeed);
        }
        if(currentTime <=0)
        {
            anim.SetFloat("Smooth", Mathf.Lerp(anim.GetFloat("Smooth"), 0.5f, Time.deltaTime * 2));
            hurtBox.SetActive(false);
        }
        CheckDist(target, targetDist, GetComponent<Ground>().moveState);
    }

    public override void Action()
    {
        if (action)
        {
            hurtBox.SetActive(true);
        }
    }
}
