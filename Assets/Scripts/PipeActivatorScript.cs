using UnityEngine;
using System.Collections;

public class PipeActivatorScript : MonoBehaviour {
	
	void OnTriggerEnter(Collider other){
		if (other.tag != "Player") return;
		PE_Controller.instance.source.PlayOneShot(PE_Controller.instance.pipe);
		Vector3 destination = Vector3.zero;
		destination.y = 6f;
		destination.x = 208.5f;
		PE_Controller.instance.peo.transform.position = destination;
	}
	
	void OnTriggerStay(Collider other){
		OnTriggerEnter(other);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(transform.position, transform.localScale);
	}
}
