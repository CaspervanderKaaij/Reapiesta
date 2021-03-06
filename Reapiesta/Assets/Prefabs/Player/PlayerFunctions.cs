﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerFunctions : MonoBehaviour
{
    [HideInInspector]
    public CharacterController cc;
    [SerializeField]
    float speed = 10;
    [SerializeField]
    float accelerationSpeed = 3;
    [SerializeField]
    float decelerationSpeed = 6;
    public ParticleSystem dustParticles;
    public float jumpHeight = 10;
    [SerializeField]
    float gravityStrength = -12;
    [SerializeField]
    float fallDeceleration = 50;

    [HideInInspector]
    public Vector3 moveV3;
    float skateSpeed = 40;
    [SerializeField]
    float skateRotSpeed = 20;
    public enum State
    {
        Foot,
        SkateBoard,
        Dash,
        Attack
    }
    public State curState = State.Foot;
    public GameObject skateBoard;
    Vector3 latePos;
    [SerializeField]
    Transform skateAngleHelper;
    [HideInInspector]
    public Cam cam;
    [Header("Skateboard Stats")]
    [SerializeField]
    float maxSkateSpeed;
    [SerializeField]
    float skateAcceleration;
    [SerializeField]
    float skateDeceleration;
    [SerializeField]
    float minSkateSpeed = 10;
    [SerializeField]
    float skateAngleSetSpeed = 10;
    bool justSkateGrounded = false;
    [SerializeField]
    float skateJumpHeight = 50;
    [HideInInspector]
    public bool grounded = false;
    public GameObject particleSkateChange;
    [HideInInspector]
    public bool canSkateJump = true;
    State stateBeforeDash = State.Foot;
    [Header("Dashing")]
    [SerializeField]
    GameObject hurtbox;
    [SerializeField]
    GameObject[] dashEffects;
    [SerializeField]
    float dashSpeed = 10;
    [SerializeField]
    Renderer[] dashInvisible;
    [SerializeField]
    GameObject particleDash;
    [SerializeField]
    GameObject landingParticle;
    bool canDash = true;
    [HideInInspector]
    public float stamina = 100;
    public UIPercentBar staminaBar;
    bool antiBounce = false;
    public enum Animation
    {
        Idle = 0,
        Walk = 1,
        Jump = 2,
        Skate = 3,
        SkateJump = 4,
        Shoot = 5,
        Attack = 6,
        Throw = 7
    }
    [Header("Animation")]
    public Animation curAnim;
    public Animator anim;
    [SerializeField]
    RuntimeAnimatorController controller;

    [Header("Ghost")]
    [SerializeField]
    Transform ghostText;
    int ghostAmount;
    [SerializeField]
    int ghostToKill = 3;

    [Header("EndScene")]
    [SerializeField]
    Image endBack;
    [SerializeField]
    GameObject[] endObjects;
    bool hasEnded = false;
    AudioSource skateSound;

    public void UpdateAnimations()
    {
        anim.SetInteger("CurAnim", (int)curAnim);
    }

    public void Start()
    {
        cc = GetComponent<CharacterController>();
        latePos = transform.position;
        cam = Camera.main.GetComponent<Cam>();
        curAnim = Animation.Idle;
        anim.runtimeAnimatorController = controller;
        Cursor.visible = false;
        skateSound = skateBoard.GetComponent<AudioSource>();
        //ghostToKill = (int)FindObjectOfType<SaveData>().enemiesLeft;
    }

    public void GhostPot(int ghost)
    {
        ghostAmount++;
        ghostText.localScale += new Vector3(-0.01f, 0.5f, 0);
        ghostText.GetComponent<Text>().text = ghostAmount.ToString();
        if (ghostAmount >= ghostToKill)
        {
            //StaticFunctions.PlayAudio(8, true);
            StartCoroutine(CheckEndState());
        }
    }

    IEnumerator CheckEndState()
    {
        hurtbox.SetActive(false);
        StaticFunctions.paused = true;
        Time.timeScale = 0.1f;
        if (FindObjectOfType<DontDestroy>() != null)
        {
            Destroy(FindObjectOfType<DontDestroy>().gameObject);
        }
        yield return new WaitForSecondsRealtime(1.5f);
        hasEnded = true;
        yield return new WaitForSecondsRealtime(1.5f);
        endBack.color = Color.white;
        endObjects[1].SetActive(true);
        endObjects[3].SetActive(true);
        yield return new WaitForSecondsRealtime(4.5f);
        Time.timeScale = 0;
        endObjects[0].SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        endObjects[2].SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        endObjects[4].SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        endObjects[5].SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        endObjects[6].SetActive(true);
        yield return new WaitForSecondsRealtime(5);
        endObjects[7].SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        StaticFunctions.paused = false;
        Time.timeScale = 1;
        StaticFunctions.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void LateUpdate()
    {
        CheckForEndState();
        latePos = transform.position;
        SetSkateSound();
    }

    void SetSkateSound()
    {
        if (grounded == false || skateSpeed < 10 || StaticFunctions.paused == true)
        {
            skateSound.volume = 0;
        }
        else
        {
            skateSound.volume = 1;
        }
        skateSound.pitch = Mathf.Max(0.5f, skateSpeed / 75);
    }

    void CheckForEndState()
    {

        if (hasEnded == true)
        {
            endBack.color = Color.Lerp(endBack.color, Color.white, Time.unscaledDeltaTime);
        }
    }

    public void StartSkateBoard()
    {
        if (cc.isGrounded == false)
        {
            grounded = false;
            if (canSkateJump == true)
            {
                curAnim = Animation.SkateJump;
                anim.Play("SkateFlip", 0);
                StaticFunctions.PlayAudio(28, true, 0);
                curState = State.SkateBoard;
                skateSpeed = 25;
                Instantiate(particleSkateChange, transform.position, Quaternion.Euler(90, 0, 0), transform);
                moveV3 = new Vector3(0, jumpHeight, 0);
                skateSpeed += 25;
                moveV3 += transform.TransformDirection(0, 0, minSkateSpeed / 5);
                transform.position += new Vector3(0, 2.1f, 0);
                //transform.Rotate(0, 0, 180);
                canSkateJump = false;
                cam.MediumShake();
            }
        }
        else
        {
            curAnim = Animation.Skate;
            anim.Play("HopOnSkateboard", 0);
            StaticFunctions.PlayAudio(28, false, 0);
            grounded = true;
            curState = State.SkateBoard;
            skateSpeed = 50;
            Instantiate(particleSkateChange, transform.position, Quaternion.Euler(90, 0, 0), transform);
            cam.SmallShake();


            moveV3 = new Vector3(0, jumpHeight / 15f, 0);
            transform.position += new Vector3(0, 2.1f, 0);
            moveV3 += transform.TransformDirection(0, 0, minSkateSpeed / 3f);

        }
    }

    public void StartDash()
    {
        if (canDash == true && Time.timeScale == 1)
        {
            StaticFunctions.PlayAudio(13, false, 0);
            stateBeforeDash = curState;
            curState = State.Dash;
            cam.MediumShake();
            for (int i = 0; i < dashInvisible.Length; i++)
            {
                dashInvisible[i].enabled = false;
            }
            Instantiate(particleDash, transform.position, Quaternion.identity);
            hurtbox.SetActive(false);
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y, 0);
            moveV3 = transform.TransformDirection(0, 0, dashSpeed);
            if (grounded == false)
            {
                canDash = false;
            }
        }
    }

    public void DashStuff()
    {
        moveV3 = Vector3.MoveTowards(moveV3, Vector3.zero, Time.deltaTime * dashSpeed);
        if (Vector3.Distance(moveV3, Vector3.zero) < dashSpeed / 4 * 3)
        {
            Instantiate(particleDash, transform.position, Quaternion.identity);
            hurtbox.SetActive(true);
            for (int i = 0; i < dashInvisible.Length; i++)
            {
                dashInvisible[i].enabled = true;
            }
            curState = stateBeforeDash;
            if (stateBeforeDash == State.SkateBoard)
            {
                skateSpeed = minSkateSpeed / 1.5f;
            }
            if (Physics.Raycast(transform.position, Vector3.down, 2) == false)
            {
                canDash = false;
                moveV3 = new Vector3(moveV3.x, jumpHeight, moveV3.z);
            }
        }
    }

    public void SkateGravity()
    {
        if (cc.isGrounded == true)
        {
            curAnim = Animation.Skate;
            canDash = true;
            canSkateJump = true;
        }
        else
        {
            curAnim = Animation.SkateJump;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.localEulerAngles.y, 0), Time.deltaTime * 3);
        if (moveV3.y > 0)
        {
            moveV3.y = Mathf.MoveTowards(moveV3.y, -687, Time.deltaTime * 67);
        }
        else
        {
            moveV3.y = Mathf.MoveTowards(moveV3.y, -687, Time.deltaTime * 124);
        }
        justSkateGrounded = false;

    }

    public void SkateForward()
    {
        curAnim = Animation.Skate;
        grounded = false;
        RaycastHit hit;
        float input = Vector2.SqrMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 2, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            //            Debug.Log(hit.transform.name);
            grounded = true;
            //sets the rotation
            skateAngleHelper.rotation = Quaternion.Lerp(skateAngleHelper.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * skateAngleHelper.rotation, Time.deltaTime * skateAngleSetSpeed);
            skateAngleHelper.localEulerAngles = new Vector3(skateAngleHelper.eulerAngles.x, transform.localEulerAngles.y, skateAngleHelper.eulerAngles.z);
            transform.rotation = skateAngleHelper.rotation;

            //sets the speed
            if (-transform.forward.y > -0.1f)
            {
                //accelerate
                if (skateSpeed < -transform.forward.y * maxSkateSpeed)
                {
                    skateSpeed = Mathf.Lerp(skateSpeed, -transform.forward.y * maxSkateSpeed, Time.deltaTime * skateAcceleration);
                }
                else//decelerate
                {
                    skateSpeed = Mathf.Lerp(skateSpeed, -transform.forward.y * maxSkateSpeed, Time.deltaTime * skateDeceleration);
                }
                skateSpeed = Mathf.Max(0, skateSpeed);
            }
            else
            {
                if (-transform.forward.y < -0.3f)
                {
                    //  Debug.Log("yes");
                    skateSpeed = Mathf.MoveTowards(skateSpeed, 0, Time.deltaTime * transform.forward.y * skateDeceleration * 50);
                }
                else
                {
                    skateSpeed = Mathf.MoveTowards(skateSpeed, minSkateSpeed, Time.deltaTime * skateDeceleration);
                }
                // Rotator for no speed
                if (-transform.forward.y < 0.6f && skateSpeed < 10) //< 10f && skateSpeed < 20)
                {
                    if (-transform.right.y > 0)
                    {
                        skateSpeed = minSkateSpeed / 100;
                        transform.Rotate(0, skateRotSpeed * Time.deltaTime, 0);
                    }
                    else
                    {
                        skateSpeed = minSkateSpeed / 100;
                        transform.Rotate(0, -skateRotSpeed * Time.deltaTime, 0);
                    }
                }
            }
            if (skateSpeed < minSkateSpeed && Input.GetAxis("Vertical") > 0)
            {
                skateSpeed = Mathf.Lerp(skateSpeed, maxSkateSpeed * 10, Time.deltaTime * skateAcceleration);
            }
            //set the actual vector
            moveV3 = transform.TransformDirection(0, 0, 1) * skateSpeed;
            //and move down toward the floor like a magnet
            if (-transform.forward.y > -0.01f)
            {
                moveV3 -= Vector3.up * 30;
            }
            else
            {
                moveV3 -= Vector3.up * 5;
            }

            //landing
            SkateLand(hit);
            justSkateGrounded = true;

            //minimum speed
            // skateSpeed = Mathf.Lerp(skateSpeed, Mathf.Max(skateSpeed,minSkateSpeed * (Input.GetAxis("Vertical"))),skateAcceleration * Time.deltaTime);

        }
        else
        {
            SkateGravity();
        }

        if (justSkateGrounded == true && Input.GetButtonDown("Jump") == true && Physics.Raycast(transform.position, Vector3.up, 2) == false)
        {
            justSkateGrounded = true;
            moveV3 += transform.TransformDirection(0, jumpHeight, 0);
            moveV3.y = skateJumpHeight;
            StaticFunctions.PlayAudio(19, false, 0);
            transform.position += new Vector3(0, 2.1f, 0);
        }

    }

    public void AntiBounceCancel()
    {
        antiBounce = false;
    }

    void SkateLand(RaycastHit hit)
    {
        if (justSkateGrounded == false)
        {
            if (antiBounce == false)
            {
                antiBounce = true;
                Invoke("AntiBounceCancel", 0.3f);
            }
            if (moveV3.y > 40)
            {
                // Instantiate(landingParticle, transform.position, transform.rotation, transform);
                cam.MediumShake();
                StaticFunctions.PlayAudio(19, false, 0);
            }
            else
            {
                cam.MediumShake();
                StaticFunctions.PlayAudio(19, false, 0);
            }
            skateSpeed /= 1.1f;
            transform.position = hit.point + new Vector3(0, 0.5f, 0);
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            canDash = true;
            canSkateJump = true;
            cc.Move(transform.TransformDirection(0, -1000 * Time.deltaTime, 0));
            moveV3.y = -1000 * Time.deltaTime;
        }
    }

    public void SkateBoost(bool shake)
    {
        skateSpeed = minSkateSpeed * 2.7f;
        if (shake == true)
        {
            cam.SmallShake();
        }
        for (int i = 0; i < dashEffects.Length; i++)
        {
            // dashEffects[i].SetActive(true);
            dashEffects[i].GetComponent<ParticleSystem>().Play();
            if (dashEffects[i].GetComponent<AudioSource>() != null && dashEffects[i].GetComponent<AudioSource>().isPlaying == false)
            {
                dashEffects[i].GetComponent<AudioSource>().Play();
            }
        }
    }

    public void StopSkateBoost()
    {
        for (int i = 0; i < dashEffects.Length; i++)
        {
            //dashEffects[i].SetActive(false);
            dashEffects[i].GetComponent<ParticleSystem>().Stop();
            if (dashEffects[i].GetComponent<AudioSource>() != null)
            {
                dashEffects[i].GetComponent<AudioSource>().Stop();
            }
        }
        //   skateSpeed = minSkateSpeed / 3;
    }

    void SetTimeBack()
    {
        if (StaticFunctions.paused == false)
        {
            Time.timeScale = 1;
        }
    }

    public void SkateAngleY()
    {
        anim.SetFloat("Blend", Mathf.Lerp(anim.GetFloat("Blend"), Input.GetAxis("Horizontal"), Time.deltaTime * 10));
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") > 0)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(transform.localEulerAngles.x, cam.transform.eulerAngles.y, transform.localEulerAngles.z), Time.deltaTime * skateRotSpeed / 40);
        }
        else
        {
            float goal = 0;
            if (Vector2.SqrMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) != 0)
            {
                goal += Input.GetAxis("Horizontal") * skateRotSpeed;
            }
            transform.Rotate(0, goal * Time.deltaTime, 0);
        }
    }

    public void Gravity()
    {

        if (cc.isGrounded == true)
        {
            if (moveV3.y < gravityStrength / 5)
            {
                // cam.SmallShake();
            }
            canDash = true;
            canSkateJump = true;
            moveV3.y = gravityStrength / 10;
            //curAnim = Animation.Idle;
        }
        else if ((int)curAnim == 1 || (int)curAnim == 0 || (int)curAnim == 4)
        {
            curAnim = Animation.Jump;
        }

        if (moveV3.y < 0)
        {
            if (moveV3.y < gravityStrength / 15)
            {
                moveV3.y = Mathf.MoveTowards(moveV3.y, gravityStrength, Time.deltaTime * fallDeceleration * 2);
            }
            else
            {
                moveV3.y = Mathf.MoveTowards(moveV3.y, gravityStrength, Time.deltaTime * fallDeceleration);
            }
        }
        else
        {
            if (moveV3.y > -gravityStrength / 30)
            {
                moveV3.y = Mathf.MoveTowards(moveV3.y, gravityStrength, Time.deltaTime * fallDeceleration);
            }
            else
            {
                moveV3.y = Mathf.MoveTowards(moveV3.y, gravityStrength, Time.deltaTime * fallDeceleration / 1.3f);
            }
        }



        if (Input.GetButtonDown("Jump") && cc.isGrounded == true)
        {
            StaticFunctions.PlayAudio(34, false, 0);
            moveV3.y = jumpHeight;
            CancelInvoke("AntiBounceCancel");
            AntiBounceCancel();
        }
    }

    public void AngleY()
    {
        float goal = transform.eulerAngles.y;
        if (Vector2.SqrMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) != 0)
        {
            goal = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
            goal += cam.transform.eulerAngles.y;
        }
        if (cc.isGrounded == true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, goal, transform.eulerAngles.z), Time.deltaTime * 20);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, goal, transform.eulerAngles.z), Time.deltaTime * 5);
        }
    }

    public void MoveForward()
    {
        Vector3 goal;
        goal = transform.TransformDirection(0, 0, Mathf.Min(1, Vector2.SqrMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))))) * speed;
        if (Vector2.SqrMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) != 0)
        {
            moveV3.x = Mathf.Lerp(moveV3.x, goal.x, Time.deltaTime * accelerationSpeed);
            moveV3.z = Mathf.Lerp(moveV3.z, goal.z, Time.deltaTime * accelerationSpeed);
            if (cc.isGrounded == true)
            {
                curAnim = Animation.Walk;
            }
        }
        else
        {
            if (cc.isGrounded == true)
            {
                curAnim = Animation.Idle;
            }
            moveV3.x = Mathf.Lerp(moveV3.x, goal.x, Time.deltaTime * decelerationSpeed);
            moveV3.z = Mathf.Lerp(moveV3.z, goal.z, Time.deltaTime * decelerationSpeed);
            if (cc.isGrounded == true && Physics.Raycast(transform.position, Vector3.down, 2, ~LayerMask.GetMask("Ignore Raycast")) == false)
            {//this makes him stop at a ledge when decelerating
                moveV3.x = 0;
                moveV3.z = 0;
            }
        }

        //this just feels better, don't question it. It stops you when the angle difference it bigger then 100
        if (cc.isGrounded == true)
        {
            float newGoal = transform.eulerAngles.y;
            if (Vector2.SqrMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))) != 0)
            {
                newGoal = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
                newGoal += cam.transform.eulerAngles.y;
            }
            if (Quaternion.Angle(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, newGoal, transform.eulerAngles.z)) > 100)
            {
                moveV3.x = 0;
                moveV3.z = 0;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, newGoal, transform.eulerAngles.z);
            }
        }
    }

    public void FinalMove()
    {
        if (antiBounce == true)
        {
            if (Input.GetAxis("Jump") == 0)
            {
                //This fixes the most obnoxious bug ever. Bouncing..
                if (Physics.Raycast(transform.position, Vector3.down, 4, LayerMask.GetMask("Ignore Raycast", "IgnoreCam")) && moveV3.y > 0)
                {
                    moveV3.y = -1000;
                }
                else
                {
                    antiBounce = false;
                    CancelInvoke("AntiBounceCancel");
                }
            }
            else
            {
                antiBounce = false;
                CancelInvoke("AntiBounceCancel");
            }
        }
        cc.Move(moveV3 * Time.deltaTime);
    }
}