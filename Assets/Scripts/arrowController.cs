using UnityEngine;
using System.Collections;

public class arrowController : MonoBehaviour {

	Animator anim;
	PE_Obj mario_Phys;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		GameObject mario = GameObject.FindGameObjectWithTag ("Player");
		mario_Phys = mario.GetComponent<PE_Obj> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h_vel = Mathf.Abs(mario_Phys.vel.x);
		anim.SetFloat ("h_vel", h_vel);
	}
}
