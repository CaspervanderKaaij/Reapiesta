using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
	[SerializeField] float timer;
	void Start()
	{
		Destroy(gameObject, timer);
	}
}
