using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitch : MonoBehaviour
{

    public int curItem = 0;
    [SerializeField]
    Text ui;
    [SerializeField]
    ScytheThrow special;
    [SerializeField]
    int specialDisable = 0;
    bool scrollInUse = false;//this is used for controller input
    [SerializeField]
    int[] offsetType;
    Cam cam;
    [SerializeField]
    Renderer[] weaponRends;
    [SerializeField]
    GameObject scytheBack;
    int zoomed = 0;


    void Start()
    {
        ui.text = transform.GetChild(curItem).name;
        cam = Camera.main.GetComponent<Cam>();
    }

    void LateUpdate()
    {
        ActivateSpecial();
        if (IsInvoking("ScrollStopper") == false && special.curState == ScytheThrow.State.Disabled && StaticFunctions.paused == false)
        {
            Scroll();
        }
        SetActives();
        SetOffsetType();
    }

    void SetOffsetType()
    {
        if (IsInvoking("StopZooming") == false)
        {
            if (Input.GetButtonDown("Throw") == true && curItem != specialDisable)
            {
                zoomed = 1;
            }
            if (Input.GetButtonUp("Throw") == true || curItem == specialDisable)
            {
                zoomed = 0;
            }
        }
        else
        {
            zoomed = 0;
        }
        cam.offsetType = offsetType[zoomed];
    }

    void StopZooming()
    {
        //invoke sh*t
    }

    void SetActives()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == curItem)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                weaponRends[i].gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
                weaponRends[i].gameObject.SetActive(false);
            }
        }
    }

    void ScrollStopper()
    {
        //another use of invoking, which uses a function that does nothing.
    }

    void Scroll()
    {
        // if (Input.mouseScrollDelta.y != 0)
        // {
        int lastItem = curItem;
        curItem += (int)Input.mouseScrollDelta.y;
        if ((int)Input.GetAxis("ScrollItem") != 0)//the controller input
        {
            if (scrollInUse == false)
            {
                curItem += (int)Input.GetAxis("ScrollItem");
                scrollInUse = true;
            }
        }
        else
        {
            scrollInUse = false;
        }
        if (curItem == specialDisable && special.curState != ScytheThrow.State.Disabled)
        {
            curItem = 1;
            lastItem = 1;
            ui.text = transform.GetChild(curItem).name;
        }
        if (curItem != lastItem)
        {
            if (lastItem == specialDisable)
            {
                StaticFunctions.PlayAudio(31, false, 0);
            }
            else
            {
                StaticFunctions.PlayAudio(32, false, 0);
            }
            ui.rectTransform.localScale = new Vector3(0.1f, 2, 1);
            Invoke("ScrollStopper", 0.3f);
        }
        if (curItem > transform.childCount - 1)
        {
            curItem = 0;
        }
        if (curItem < 0)
        {
            curItem = transform.childCount - 1;
        }
        ui.text = transform.GetChild(curItem).name;
        if (Time.timeScale == 0.25f && StaticFunctions.paused == false)//EDIT THIS, IT'S HARDCODED
        {
            Time.timeScale = 1;
        }
        //}
    }

    void ActivateSpecial()
    {
        if (curItem == specialDisable && Input.GetButtonDown("NewThrow"))
        {
            // special.SetActive(false);
            curItem++;
            ui.text = transform.GetChild(curItem).name;
            Invoke("StopZooming", 0.2f);
        }
        if (curItem == specialDisable && special.curState != ScytheThrow.State.Disabled)
        {
            // Scroll();
            curItem = 1;
            ui.text = transform.GetChild(curItem).name;
        }

        if (special.curState == ScytheThrow.State.Disabled)
        {
            if (curItem == specialDisable)
            {
                scytheBack.SetActive(false);
            }
            else
            {
                scytheBack.SetActive(true);
            }
        }
    }
}
