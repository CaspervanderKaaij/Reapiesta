using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSetter : MonoBehaviour
{

    bool active = false;
    [SerializeField] GameObject visual;
    [SerializeField] GameObject normalMenu;
    [SerializeField] GameObject optionMenu;
    void Start()
    {
        StaticFunctions.paused = false;
        Time.timeScale = 1;
        visual.SetActive(false);
        active = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (active == false)
        {
            if (Input.GetButtonDown("Pause") == true && StaticFunctions.paused == false && Time.timeScale == 1)
            {
                normalMenu.SetActive(true);
                optionMenu.SetActive(false);
                StaticFunctions.paused = true;
                Time.timeScale = 0;
                visual.SetActive(true);
                active = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            if (Input.GetButtonDown("Pause") == true)
            {
                StaticFunctions.paused = false;
                Time.timeScale = 1;
                visual.SetActive(false);
                active = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
