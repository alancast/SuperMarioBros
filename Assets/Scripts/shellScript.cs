using UnityEngine;
using System.Collections;

public class shellScript : MonoBehaviour {
	public Vector3 		shellVel = new Vector3(16,0,0);
	PE_Obj 				this_shell;

	bool below(Collider other){
		if (Physics.Raycast(transform.position, 
		                    new Vector3(0, 1, 0), 
		                    transform.collider.bounds.size.y + .05f)) return true;
		else return false;
	}
	
	void Awake(){
		this_shell = GetComponent<PE_Obj> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Item") return;
		if (other.tag == "Player"){
			if (below(other)){
				this_shell.vel.x = 0;
				return;
			}
			if (PE_Controller.instance.vel.x > 0){
				this_shell.vel = shellVel;
			}
			else{
				this_shell.vel = -shellVel;
			}
			return;
		}
		this_shell.vel.x = -this_shell.vel0.x;
	}
}
