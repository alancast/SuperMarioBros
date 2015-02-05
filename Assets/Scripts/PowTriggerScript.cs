using UnityEngine;
using System.Collections;

public class PowTriggerScript : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player"){
			PE_Controller.instance.powed = true;
		}
	}
}
