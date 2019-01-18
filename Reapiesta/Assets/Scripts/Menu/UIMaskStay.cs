using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaskStay : MonoBehaviour
{

    Vector2 initialPos = Vector2.zero;
    Vector3 initialRot = Vector3.zero;
    Vector3 initialScale = Vector3.one;
	
    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.eulerAngles;
        initialScale = transform.lossyScale;
    }

    void LateUpdate()
    {
        Transform oldParent = transform.parent;
        transform.SetParent(null);
        transform.position = initialPos;
        transform.eulerAngles = initialRot;
        transform.localScale = initialScale;
        transform.SetParent(oldParent);
    }
}
