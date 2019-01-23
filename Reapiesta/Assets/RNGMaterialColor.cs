using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGMaterialColor : MonoBehaviour {

	void Start () {
		Color rngColor = Color.red;
		int rng = 0;
		rng = Random.Range(0,7);
		switch (rng)
		{
			case 0:
			rngColor = Color.red;
			break;
			case 1:
			rngColor = Color.blue;
			break;
			case 2:
			rngColor = Color.green;
			break;
			case 3:
			rngColor = Color.yellow;
			break;
			case 4:
			rngColor = Color.cyan;
			break;
			case 5:
			rngColor = Color.magenta;
			break;
			case 6:
			rngColor = Color.grey;
			break;
			case 7:
			rngColor = Color.white;
			break;
		}
		transform.GetChild(0).GetComponent<Renderer>().material.color = rngColor;
		rng = Random.Range(0,5);
		switch (rng)
		{
			case 0:
			rngColor = Color.red;
			break;
			case 1:
			rngColor = Color.blue;
			break;
			case 2:
			rngColor = Color.green;
			break;
			case 3:
			rngColor = Color.yellow;
			break;
			case 4:
			rngColor = Color.cyan;
			break;
			case 5:
			rngColor = Color.magenta;
			break;
		}
		transform.GetChild(1).GetComponent<Renderer>().material.color = rngColor;
	}
}
