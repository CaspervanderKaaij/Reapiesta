using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{

    [SerializeField]
    Vector3 goalScale = Vector3.one;
    [SerializeField]
    float speed = 1;
    bool hasStarted = false;


    void Update()
    {
        if(hasStarted == true){
        transform.localScale = Vector3.Lerp(transform.localScale, goalScale,speed * Time.unscaledDeltaTime);
        } else {
            hasStarted = true;
        }
    }
}