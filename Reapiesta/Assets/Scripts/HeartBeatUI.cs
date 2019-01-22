using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatUI : MonoBehaviour {

public RectTransform[] hearts;
public float speed = 0.1f;

	void Start () {
		InvokeRepeating("ScaleHeart",0,speed);
	}
	
	void ScaleHeart () {
		for (int i = 0; i < hearts.Length; i++)
		{	
		hearts[i].localScale *= 1.25f;
		}
	}
}
