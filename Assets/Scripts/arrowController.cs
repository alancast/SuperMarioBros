using UnityEngine;
using System.Collections;

public class arrowController : MonoBehaviour {

	Animator anim;
	PE_Obj mario_Phys;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h_vel = Mathf.Abs(PE_Controller.instance.vel.x);
		anim.SetFloat ("h_vel", h_vel);
	}
}
