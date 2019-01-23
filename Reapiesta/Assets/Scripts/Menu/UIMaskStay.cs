using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class UIMaskStay : MonoBehaviour
{

    Vector2 initialPos = Vector2.zero;
    Vector3 initialRot = Vector3.zero;
    Vector3 initialScale = Vector3.one;

    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.eulerAngles;
      //  if (Application.isEditor == false)
       // {
            initialScale = transform.lossyScale;
      //  }
    }

    void LateUpdate()
    {
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
}
