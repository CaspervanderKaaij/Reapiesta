using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheThrow : MonoBehaviour
{

    Transform player;
    [SerializeField] float speed = 10;
    [SerializeField] float returnSpeed = 10;
    Vector3 goal;
    [SerializeField] float range = 100;
    PlayerFunctions pf;
    public enum State
    {
        Disabled,
        Normal,
        GoBack
    }
    public State curState = State.Disabled;
    [SerializeField] List<Renderer> rend;
    Cam cam;
    [SerializeField] GameObject hurtbox;
    Vector3 lastPos;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField]
    float anticipationTime = 0.3f;
    [SerializeField]
    float recoverTime = 0.1f;
    [SerializeField]
    GameObject scytheBack;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        cam = Camera.main.GetComponent<Cam>();
        lastPos = transform.position;
        pf = player.GetComponent<PlayerFunctions>();

    }

    void Update()
    {
        switch (curState)
        {
            case State.Disabled:
                DisabledStuff();
                if (StaticFunctions.paused == false)
                {
                    if (Input.GetButtonDown("NewThrow") == true && pf.curState == PlayerFunctions.State.Foot)
                    {
                        Invoke("StartThrow", anticipationTime);
                        pf.curState = PlayerFunctions.State.Attack;
                        pf.anim.Play("ScytheThrow");
                        Invoke("SetPlayerState", anticipationTime + recoverTime);
                    }
                    if (Input.GetButtonDown("Throw") == true && pf.curState == PlayerFunctions.State.Foot && FindObjectOfType<ItemSwitch>().curItem == 0)
                    {
                        Invoke("StartThrow", anticipationTime);
                        pf.curState = PlayerFunctions.State.Attack;
                        pf.anim.Play("ScytheThrow");
                        Invoke("SetPlayerState", anticipationTime + recoverTime);
                    }
                    if (Input.GetButtonDown("Throw") == true && pf.curState == PlayerFunctions.State.SkateBoard && FindObjectOfType<ItemSwitch>().curItem == 0)
                    {
                        Invoke("StartThrow", anticipationTime);
                        pf.curState = PlayerFunctions.State.Attack;
                        pf.anim.Play("ScytheThrowSkate");
                        Invoke("SetPlayerSkateState", anticipationTime + recoverTime);
                    }
                    if (Input.GetButtonDown("NewThrow") == true && pf.curState == PlayerFunctions.State.SkateBoard)
                    {
                        Invoke("StartThrow", anticipationTime);
                        pf.curState = PlayerFunctions.State.Attack;
                        pf.anim.Play("ScytheThrowSkate");
                        Invoke("SetPlayerSkateState", anticipationTime + recoverTime);
                    }
                }
                break;
            case State.Normal:
                NormalThrow();
                RotateScythe();
                RayCollider();
                break;
            case State.GoBack:
                MoveToPlayer();
                Catch();
                RotateScythe();
                break;
        }
    }

    void SetPlayerState()
    {
        pf.curState = PlayerFunctions.State.Foot;
    }
    void SetPlayerSkateState()
    {
        pf.curState = PlayerFunctions.State.SkateBoard;
    }

    void RayCollider()
    {
        RaycastHit hit;
        if (Physics.Raycast(lastPos, lastPos - transform.position, out hit, Vector3.Distance(transform.position, lastPos), ~LayerMask.GetMask("IgnoreCam", "Ignore Raycast", "IgnoreTrigger", "ClothCollider")))
        {
            curState = State.GoBack;
            StaticFunctions.PlayAudio(22, false, 0);
            GetComponent<AudioSource>().Play();
            cam.SmallShake();
        }
        else if (Physics.Raycast(transform.position, transform.position - lastPos, out hit, Vector3.Distance(transform.position, lastPos), ~LayerMask.GetMask("IgnoreCam", "Ignore Raycast", "IgnoreTrigger", "ClothCollider")))
        {
            curState = State.GoBack;
            StaticFunctions.PlayAudio(22, false, 0);
            GetComponent<AudioSource>().Play();
            cam.SmallShake();
        }
        // Debug.DrawRay(lastPos, lastPos - transform.position, Color.red, 1);
        lastPos = transform.position;
    }

    void RotateScythe()
    {
        for (int i = 0; i < rend.Count; i++)
        {
            rend[i].transform.Rotate(1000 * Time.deltaTime, 0, 0);
        }
    }

    void DisabledStuff()
    {
        for (int i = 0; i < rend.Count; i++)
        {
            rend[i].enabled = false;
        }
        transform.position = player.position;
        if (hurtbox != null)
        {
            hurtbox.SetActive(false);
        }
    }

    void MoveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, returnSpeed * Time.deltaTime);
    }

    void StartThrow()
    {
        scytheBack.SetActive(false);
        lastPos = transform.position;
        for (int i = 0; i < rend.Count; i++)
        {
            rend[i].enabled = true;
        }
        curState = State.Normal;
        cam.SmallShake();
        if (hurtbox != null)
        {
            hurtbox.SetActive(true);
        }
        StaticFunctions.PlayAudio(22, false, 0);
        GetComponent<AudioSource>().Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cam.transform.forward, out hit, 10, ~LayerMask.GetMask("IgnoreCam", "Ignore Raycast")))
        {
            float ySaveCam = cam.transform.eulerAngles.x;
            cam.transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);
            goal = cam.transform.position + cam.transform.forward * range + cam.transform.TransformDirection(offset);
            cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, ySaveCam, cam.transform.eulerAngles.z);
            //transform.position += (transform.position - hit.point);
            //Debug.Log("nani?");
        }
        else
        {
            goal = player.position + cam.transform.forward * range + cam.transform.TransformDirection(offset);
        }
    }

    void NormalThrow()
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, goal) < 10)
        {
            curState = State.GoBack;
        }
    }

    void Catch()
    {
        if (Vector3.Distance(transform.position, player.position) < 5)
        {
            scytheBack.SetActive(true);
            StaticFunctions.PlayAudio(21, true, 0);
            GetComponent<AudioSource>().Stop();
            curState = State.Disabled;
            cam.SmallShake();
        }
    }
}
