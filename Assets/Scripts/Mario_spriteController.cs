using UnityEngine;
using System.Collections;

public class Mario_spriteController : MonoBehaviour {

	Animator anim;
	PE_Obj marioPhys;
	BoxCollider marioCollider;
	Vector3 bigColliderDim = new Vector3(1f, 1.6f, 1f);
	Vector3 smallColliderDim = new Vector3(.85f, 1f, 1f);
	MarioState state;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		marioPhys = GetComponent<PE_Obj> ();
		marioCollider = GetComponent<BoxCollider> ();
		state = GetComponent<PE_Controller> ().state;
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
		state = GetComponent<PE_Controller> ().state;
		if (state == MarioState.Small) {
			marioCollider.size = smallColliderDim;
		} else {
			marioCollider.size = bigColliderDim;
		}
	}

	void Movement (){
		anim.SetFloat ("x_vel", marioPhys.vel0.x);
		bool jumping = false;

		if (marioPhys.vel0.y != 0) {
			jumping = true;
		}

		anim.SetBool ("jumping", jumping);

	}
}
