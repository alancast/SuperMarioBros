using UnityEngine;
using System.Collections;

public class EndStarScript : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag != "Player") return;
		Application.LoadLevel("_Menu_Scene");
	}
}
