using UnityEngine;
using System.Collections;

public class killCharacter : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			CameraMGR.lives -= 1;
			CameraMGR.instance.livesText.text = CameraMGR.lives.ToString();
			Application.LoadLevel("_Scene_Alex_7");
		}
	}
	
}
