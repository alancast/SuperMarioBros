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
	
	public void blockHit(){
		if(!wasHit){
			Instantiate(hitItem, itemPos, Quaternion.identity);
			PE_Controller.instance.cantHitTil = Time.time + .1f;
		}
		
		wasHit = true;
		PE_Controller.instance.isJumping = false;
		anim.SetBool ("wasHit", wasHit);
	}

}
