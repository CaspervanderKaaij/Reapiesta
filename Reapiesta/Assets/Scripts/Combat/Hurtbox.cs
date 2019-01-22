﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class Hurtbox : MonoBehaviour
{

    [SerializeField] float damage = 1;
    [SerializeField] int team = 0;
    [SerializeField] bool destroyOnHit = true;
    [SerializeField] bool bullet = false;
    [SerializeField] GameObject hitParticle;

    void OnTriggerEnter(Collider other)
    {
        Hitbox hit = other.GetComponent<Hitbox>();
        if (hit != null)
        {
            if (team != hit.team)
            {
                hit.Hit(damage);
                if (destroyOnHit == true)
                {
                    Destroy(gameObject);
                }
                // Debug.Log(other.name + " got hit!");
            }
            else
            {
                //Destroy(gameObject);
                //  Debug.Log("Same team");
            }
        } else if (bullet == true && other.name != "Player")
        {
            Destroy(gameObject);
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }
}
