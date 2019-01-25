using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour {
	public uint lives = 10;
	public uint enemiesLeft = 50;

	void Awake() {
		lives = SaveLoad.LoadManager(true);
		enemiesLeft = SaveLoad.LoadManager(false);
	}
}
