using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (FindObjectsOfType<DontDestroy>().Length > 1)
        {
            Destroy(FindObjectsOfType<DontDestroy>()[0].gameObject);
        }
    }
}
