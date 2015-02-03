using UnityEngine;
using System.Collections;

public class KillFallTimeScript : MonoBehaviour {
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "Player") return;
		if (PE_Controller.instance.peo.vel.y < -1f){
			PE_Controller.instance.peo.vel.y = -1f;
		}
	}
	
	void OnTriggerStay(Collider other){
		OnTriggerEnter(other);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, transform.localScale);
	}
}
