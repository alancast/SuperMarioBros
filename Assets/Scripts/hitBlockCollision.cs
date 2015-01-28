using UnityEngine;
using System.Collections;

public class hitBlockCollision : MonoBehaviour {

	Animator anim;
	bool wasHit = false;
	public float 		raycastDistance;
	public GameObject 	hitItem;
	public Vector3 		itemPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		itemPos = this.transform.position;
		itemPos.y += 1f;
	}
	
	bool onTop(Collider other){
		if (Physics.Raycast(other.transform.position, 
		                    new Vector3(0, 1, 0), 
		                    other.transform.collider.bounds.size.y + raycastDistance)) return true;
		else return false;
	}
	
	bool rightTop(Collider other){
		Vector3 origin = other.transform.position;
		origin.x += other.transform.collider.bounds.size.x/2;
		if (Physics.Raycast(origin, 
		                    new Vector3(0, 1, 0), 
		                    other.transform.collider.bounds.size.y + raycastDistance)) return true;
		else return false;
	}
	
	bool leftTop(Collider other){
		Vector3 origin = other.transform.position;
		origin.x -= other.transform.collider.bounds.size.x/2;
		if (Physics.Raycast(origin, 
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
		if(onTop(other) || rightTop(other) || leftTop(other)){

			if(!wasHit){
				Instantiate(hitItem, itemPos, Quaternion.identity);
			}

			wasHit = true;
			PE_Controller.instance.isJumping = false;
			anim.SetBool ("wasHit", wasHit);
		}

	}

}
