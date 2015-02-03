using UnityEngine;
using System.Collections;

public class EndGameScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.H)){
			Application.LoadLevel("_Menu_Scene");
		}
	}
}
