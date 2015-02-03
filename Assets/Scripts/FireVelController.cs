using UnityEngine;
using System.Collections;

public class FireVelController : MonoBehaviour {
	PE_Obj this_FireBall;
	public Vector3 fireSpeed;


	// Use this for initialization
	void Start () {
		this_FireBall = GetComponent<PE_Obj> ();
		//this_FireBall.acc = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		//this_FireBall.acc = Vector3.zero;
		this_FireBall.vel = fireSpeed;
	}
}
