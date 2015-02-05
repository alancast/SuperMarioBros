using UnityEngine;
using System.Collections;

public class TailScript : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag != "Goomba") return;
		print ("trigged");
	}
	
	void OnTriggerStay(Collider other){
		OnTriggerEnter(other);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, transform.localScale);
	}
}
