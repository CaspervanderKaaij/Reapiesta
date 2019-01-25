using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGMaterialColor : MonoBehaviour
{
    [SerializeField] Color[] colorBank;
    [SerializeField] Color[] colorBank1;
    [SerializeField] Color test1;
	[SerializeField] Color test2;

    void Start()
    {
        int index0 = Random.Range(0, colorBank.Length);
        Renderer r0 = transform.GetChild(0).GetComponent<Renderer>();
        r0.material.color = colorBank[index0];

        int index1 = Random.Range(0, colorBank1.Length);
        Renderer r1 = transform.GetChild(1).GetComponent<Renderer>();
        r1.material.color = colorBank1[index1];
		test1 = colorBank[index0];
		test2 = colorBank1[index1];       
		/* 
        int index2 = Random.Range(0, colorBank.Length);
        Renderer r2 = transform.GetChild(0).GetComponent<Renderer>();
        r0.material.color = colorBank[index2];
		*/
    }
}
