using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class UIMaskStay : MonoBehaviour
{

    Vector2 initialPos = Vector2.zero;
    Vector3 initialRot = Vector3.zero;
    Vector3 initialScale = Vector3.one;
    Vector2 lastRes;

    void Start()
    {
        lastRes = new Vector2(Screen.width, Screen.height);
        SetInitials();
    }

    void LateUpdate()
    {
        if (lastRes.x != Screen.width || lastRes.y != Screen.height)
        {
            SetInitials();
            lastRes.x = Screen.width;
            lastRes.y = Screen.height;
        }
        // Debug.Log(lastRes);
        //  Debug.Log(new Vector2(Screen.currentResolution.width,Screen.currentResolution.height));
        //  if (Application.isEditor == false)
        // {
        Transform oldParent = transform.parent;
        transform.SetParent(null);
        transform.localScale = initialScale;
        transform.SetParent(oldParent);
        //  }
        transform.position = initialPos;
        transform.eulerAngles = initialRot;
    }

    void SetInitials()
    {
        initialPos = transform.position;
        initialRot = transform.eulerAngles;
        initialScale = transform.lossyScale;
    }
}
