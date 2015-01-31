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
	
	bool below(){
		//return true if middle is under
		//or if only left or right is under
		Vector3 origin = transform.position;
		origin.x += collider.bounds.size.x/2;
		bool middle, left, right;
		right = Physics.Raycast(origin, new Vector3(0, -1, 0), collider.bounds.size.y + .1f);
		origin.x -= collider.bounds.size.x;
		left = Physics.Raycast(origin, new Vector3(0, -1, 0), collider.bounds.size.y + .1f);
		middle = Physics.Raycast(transform.position, new Vector3(0, -1, 0), collider.bounds.size.y +.1f);
		if (middle) return true;
//		if (left && !middle && !right) return true;
//		if (right && !middle && !left) return true;
		return false;
	}

	void OnTriggerEnter(Collider other) {
		PE_Dir dir = other.gameObject.GetComponent<PE_Obj> ().dir;
		
		if(below ()){

			if(!wasHit){
				Instantiate(hitItem, itemPos, Quaternion.identity);
			}

			wasHit = true;
			PE_Controller.instance.isJumping = false;
			anim.SetBool ("wasHit", wasHit);
		}

	}

}
