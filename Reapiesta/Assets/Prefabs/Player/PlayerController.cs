﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    [HideInInspector] public PlayerFunctions pf;
    [SerializeField] ItemSwitch itemSwitch;
    [SerializeField] Shoot shoot;
    [SerializeField] ScytheThrow scytheThrow;
    [SerializeField] ScytheAttack scytheAttack;
    PostProcessingBehaviour pp;
    bool canBoost = true;
    bool canDash = true;
    //set these in playerfuncion
    [SerializeField] float dashLagTime = 0.5f;
    [SerializeField] float boostStaminaCon = 40;
    [SerializeField] float staminaReloadTime = 30;
    [SerializeField] float dashCon = 30;
    bool canReplenishStamina = true;
    float lastStamina;
    [SerializeField] float staminaStopTime = 0.6f;


    void Start()
    {
        pf = GetComponent<PlayerFunctions>();
        pp = Camera.main.GetComponent<PostProcessingBehaviour>();
        pf.StopSkateBoost();
        canBoost = false;
        lastStamina = pf.stamina;
    }
    void Update()
    {
        if (StaticFunctions.paused == false)
        {
            Controll();
        }
        pf.UpdateAnimations();
    }

    void Controll()
    {
        if (Input.GetButtonUp("Dash"))
        {
            canBoost = true;
        }
        switch (pf.curState)
        {
            case PlayerFunctions.State.SkateBoard:
                pf.skateBoard.SetActive(true);
                pf.dustParticles.Stop();
                pf.SkateForward();
                pf.SkateAngleY();
                // Gravity();
                pf.FinalMove();
                if (Input.GetButtonDown("Fire2"))
                {
                    StaticFunctions.PlayAudio(22, false, 0);
                    pf.curState = PlayerFunctions.State.Foot;
                    pf.StopSkateBoost();
                    canBoost = false;
                    transform.eulerAngles = new Vector3(0, Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y, 0);
                    Instantiate(pf.particleSkateChange, transform.position, Quaternion.Euler(90, 0, 0), transform);
                    pf.cam.MediumShake();

                    if (pf.canSkateJump == false)
                    {
                        pf.moveV3 = new Vector3(pf.moveV3.x, pf.jumpHeight, pf.moveV3.z);
                    }
                }
                if (Input.GetButtonDown("Dash") == true && pf.stamina >= 100)
                {
                    canBoost = true;
                    pf.stamina -= 10;
                    if (pf.grounded == true)
                    {
                        pf.SkateBoost(true);
                        StaticFunctions.PlayAudio(33, false, 0);
                        pp.profile.motionBlur.enabled = true;
                    }
                    else
                    {
                        if (canDash == true)
                        {
                            pf.StartDash();
                            Invoke("SetCanDash", dashLagTime);
                        }
                    }
                }
                // pf.StartDash();


                if (Input.GetAxis("Dash") != 0 && pf.stamina > 10 * Time.deltaTime && canBoost == true)
                {
                    if (pf.grounded == true)
                    {
                        pf.SkateBoost(true);
                        pf.stamina -= boostStaminaCon * Time.deltaTime;
                        pp.profile.motionBlur.enabled = true;
                    }
                }
                if (Input.GetButtonUp("Dash") == true || pf.grounded == false || pf.stamina < 10 * Time.deltaTime)
                {
                    pf.StopSkateBoost();
                    canBoost = false;
                    pp.profile.motionBlur.enabled = false;
                }

                break;


            case PlayerFunctions.State.Dash:
                pf.dustParticles.Stop();
                pf.DashStuff();
                pf.FinalMove();
                break;


            case PlayerFunctions.State.Foot:
                pf.grounded = true;
                if (pp.profile.motionBlur.enabled == true)
                {
                    pp.profile.motionBlur.enabled = false;
                    pf.StopSkateBoost();
                    canBoost = false;
                }
                //here
                pf.skateBoard.SetActive(false);
                pf.MoveForward();
                pf.AngleY();
                if (Input.GetButtonDown("Fire2"))
                {
                    pf.StartSkateBoard();
                }
                if (Input.GetButtonDown("Dash"))
                {
                    if (canDash == true && pf.stamina >= dashCon)
                    {
                        pf.stamina -= dashCon;
                        canDash = false;
                        Invoke("SetCanDash", dashLagTime);
                        pf.StartDash();
                    }
                    // pp.profile.motionBlur.enabled = true;
                    // Camera.main.fieldOfView = 40;
                    //here
                }
                if (pf.cc.isGrounded == true)
                {
                    if (pf.dustParticles.isStopped == true)
                    {
                        pf.dustParticles.Play();
                    }
                }
                else
                {
                    pf.dustParticles.Stop();
                }
                pf.Gravity();
                pf.FinalMove();
                break;
            case PlayerFunctions.State.Attack:
                pf.Gravity();
                pf.moveV3.x = 0;
                pf.moveV3.z = 0;
                pf.FinalMove();
                if (pf.anim.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
                {
                    pf.curState = PlayerFunctions.State.Foot;
                }
                break;
        }
        if (pf.stamina != lastStamina)
        {
            CancelInvoke("ResetStamina");
            Invoke("ResetStamina", staminaStopTime);
        }
        pf.staminaBar.curPercent = pf.stamina;
        if (IsInvoking("ResetStamina") == false)
        {
            pf.stamina = Mathf.MoveTowards(pf.stamina, 100, Time.deltaTime * staminaReloadTime);
        }
        lastStamina = pf.stamina;
    }

    void ResetStamina()
    {
        //just don't delete it.
    }

    void SetCanDash()
    {
        canDash = true;
    }


}
