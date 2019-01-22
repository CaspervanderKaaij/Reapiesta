using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWeapon : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] int Clipsize;
    [SerializeField] Text ammoAmountText;
    int currentAmmoAmount;
    [SerializeField] float forceAmount;
    [SerializeField] Transform bullet;
    [SerializeField] Transform barrelEnd;
    [Header("Partical")]
    [SerializeField] GameObject shootPartical;
    Transform camPos;
    RaycastHit hit;
    [SerializeField] float rayLenght;

    //By Casper :D
    [SerializeField] Transform player;
    Cam cam;
    [SerializeField] PlayerFunctions pf;
    [SerializeField] GameObject muzzleFlash;
    void Start()
    {
        camPos = Camera.main.transform;
        currentAmmoAmount = Clipsize;
        cam = GameObject.FindObjectOfType<Cam>();
        pf = player.GetComponent<PlayerFunctions>();
    }

    void Update()
    {
        Shoot();
        Reload();
    }

    void Reload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            // set ammoAmount to max Ammo
            currentAmmoAmount = Clipsize;
            // trigger the UIFunction() for ammoUI
            UIFunction();
        }
    }

    void LateUpdate()
    {
        if (IsInvoking("PlayerRot") == true)
        {
            player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, cam.transform.eulerAngles.y, player.transform.eulerAngles.z);
        }
    }

    void Shoot()
    {
        if (pf.curState == PlayerFunctions.State.Foot)
        {
            pf.anim.SetFloat("Blend", 1);
        }
        //	when click shoot
        if (Input.GetButtonDown("Attack") && IsInvoking("PlayerRot") == false)
        {
            Invoke("ShootStuff", 0.1f);
            player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, cam.transform.eulerAngles.y, player.transform.eulerAngles.z);
        }
    }

    void ShootStuff()
    {
        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, cam.transform.eulerAngles.y, player.transform.eulerAngles.z);
        if (pf.curState != PlayerFunctions.State.SkateBoard)
        {
            pf.anim.Play("Shoot", 0, 0);
        } else
        {
            pf.anim.Play("ShootSkate", 0, 0);
        }
        Instantiate(muzzleFlash, barrelEnd.position, barrelEnd.rotation,barrelEnd);
        Invoke("PlayerRot", 0.25f);
        cam.SmallShake();
        Transform newBullet = Instantiate(bullet, barrelEnd.position + (camPos.forward * 3), barrelEnd.rotation);
        Rigidbody addRigid = newBullet.GetComponent<Rigidbody>();
        // trigger the UIFunction
        UIFunction();
        // if the raycast hit a thing
        if (Physics.Raycast(camPos.position, camPos.forward, out hit, rayLenght))
        {
            //	move to the point the raycast hit an object
            //  addRigid.velocity = (hit.point - transform.position).normalized * forceAmount;
            //  addRigid.rotation = Quaternion.LookRotation(addRigid.velocity);
            addRigid.transform.LookAt(hit.point);
            addRigid.AddForce(addRigid.transform.forward * forceAmount * 75);
        }
        else
        {
            // move to the point you click
            addRigid.AddForce(Camera.main.transform.forward * forceAmount * 250);

        }

        if (currentAmmoAmount <= 0)
        {
            currentAmmoAmount = 0;
        }
    }

    void PlayerRot()
    {
        //   pf.curState = PlayerFunctions.State.Foot;
    }

    void UIFunction()
    {
        // subtract bullet ammount
        ammoAmountText.text = currentAmmoAmount + "/" + "Ammo".ToString();
    }
}
