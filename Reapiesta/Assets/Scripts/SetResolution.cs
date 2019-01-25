using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetResolution : MonoBehaviour
{

    public void SetRes()
    {
        switch (GetComponent<Dropdown>().value)
        {
            case 0:
                Screen.SetResolution(3840, 2160, Screen.fullScreen, 60);
                break;
            case 1:
				Screen.SetResolution(1920, 1080, Screen.fullScreen, 60);
                break;
            case 2:
				Screen.SetResolution(1280, 720, Screen.fullScreen, 60);
                break;
        }
    }
}
