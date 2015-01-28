using UnityEngine;
using System.Collections;

public class killCharacter : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			CameraMGR.instance.lives -= 1;
			CameraMGR.instance.livesText.text = CameraMGR.instance.lives.ToString();
		}
	}
	
}
