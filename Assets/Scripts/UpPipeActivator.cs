using UnityEngine;
using System.Collections;

public class UpPipeActivator : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag != "Player") return;
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
			Vector3 destination = Vector3.zero;
			destination.y = -.5f;
			destination.x = 146f;
			PE_Controller.instance.peo.transform.position = destination;
		}
	}
	
	void OnTriggerStay(Collider other){
		OnTriggerEnter(other);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(transform.position, transform.localScale);
	}
}
