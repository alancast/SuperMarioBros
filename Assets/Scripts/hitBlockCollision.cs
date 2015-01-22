using UnityEngine;
using System.Collections;

public class hitBlockCollision : MonoBehaviour {

	Animator anim;
	bool wasHit = false;
	public float 		raycastDistance;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	bool onTop(Collider other){
		if (Physics.Raycast(other.transform.position, 
		                    new Vector3(0, 1, 0), 
		                    other.transform.collider.bounds.size.y + raycastDistance)) return true;
		else return false;
	}

	void OnTriggerEnter(Collider other) {
		PE_Dir dir = other.gameObject.GetComponent<PE_Obj> ().dir;
		//print ("goo");

//		if (dir == PE_Dir.up) {
//				wasHit = true;
//				anim.SetBool ("wasHit", wasHit);
//		}
		if(onTop(other)){
			wasHit = true;
			anim.SetBool ("wasHit", wasHit);
		}
	}

}
