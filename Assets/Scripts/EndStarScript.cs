using UnityEngine;
using System.Collections;

public class EndStarScript : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		Application.LoadLevel("_Menu_Scene");
	}
}
