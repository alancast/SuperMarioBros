﻿using UnityEngine;
using System.Collections;

public class MenuScreenScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			Application.LoadLevel("_Scene_Alex_7");
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)){
			print ("button 2 pressed");
//			Application.loadedLevel("_Scene_Alex_7");
		}
	
	}
}
