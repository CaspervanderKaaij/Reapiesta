using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Range : GeneralEnemyCode
{
    Transform player;
    Transform throwPos;
    public Vector3 target;
    float mintargetDist;
    [SerializeField] float forceAmount;
    public RangeStats rangeStats;
    public override void Start()
    {
        anim = GetComponent<Animator>();
        throwPos = transform.GetChild(0);
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        currentTime = rangeStats.attackSpeed;
        mintargetDist = rangeStats.mintargetDist;
        forceAmount = rangeStats.forceAmount;
    }
    public override void Update()
    {
        Timer(rangeStats.attackSpeed);
        CheckDist(target, targetDist, GetComponent<Ground>().moveState);
    }
    public void Attack()
    {
        Timer(rangeStats.attackSpeed);
        anim.SetFloat("Smooth", Mathf.Lerp(anim.GetFloat("Smooth"),1f,Time.deltaTime * 2));
    }

    public override void Action()
    {
        var index = Random.Range(0, rangeStats.bottle.Length);
        Transform newBottle = Instantiate(rangeStats.bottle[index], throwPos.position, throwPos.rotation);
        Rigidbody addRigid = newBottle.GetComponent<Rigidbody>();
        addRigid.velocity = (target - transform.position).normalized * forceAmount;
        addRigid.rotation = Quaternion.LookRotation(addRigid.velocity);
    }

}
