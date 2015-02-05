using UnityEngine;
using System.Collections;

public class TailScript : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if (other.tag != "Goomba" && other.tag != "HitBlock") return;
		if (other.tag == "HitBlock"){
			if(Time.time > PE_Controller.instance.cantHitTil){
				hitBlockCollision block_instance = other.GetComponent<hitBlockCollision>();
				block_instance.blockHit();
			}
		}
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
