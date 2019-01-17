using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensField : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (transform.parent.gameObject.GetComponent<Melee>())
            {
                transform.parent.gameObject.GetComponent<Melee>().target = other.transform.position;
            }
            else
            {
                print("the player enters the area");
                transform.parent.gameObject.GetComponent<Range>().target = other.transform.position;
            }
            transform.parent.gameObject.GetComponent<Ground>().moveState = MoveState.chasing;
            transform.parent.gameObject.GetComponent<Ground>().target = other.transform.position;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (transform.parent.gameObject.GetComponent<Melee>())
            {
                transform.parent.gameObject.GetComponent<Melee>().target = other.transform.position;
            }
            else
            {
                print("I see the player");
                transform.parent.gameObject.GetComponent<Range>().target = other.transform.position;
                transform.parent.gameObject.GetComponent<Range>().Attack();
            }
            transform.parent.gameObject.GetComponent<Ground>().moveState = MoveState.chasing;
            transform.parent.gameObject.GetComponent<Ground>().target = other.transform.position;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            // set the moveState to idle
            transform.parent.gameObject.GetComponent<Ground>().moveState = MoveState.idle;
            // set the target to it self
            transform.parent.gameObject.GetComponent<Ground>().target = transform.position;
        }
    }
}
