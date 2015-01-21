using UnityEngine;
using System.Collections;

public class Mario_spriteController : MonoBehaviour {

	Animator anim;
	PE_Obj marioPhys;
	PE_Obj ground;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		marioPhys = GetComponent<PE_Obj> ();
		ground = marioPhys.ground;
	}
	
	// Update is called once per frame
	void Update () {
		Movement ();
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
