using UnityEngine;
using System.Collections;

public class goombaScript : MonoBehaviour {

	public float x_vel = 1f;
	PE_Obj this_Goomba;
	
	// Use this for initialization
	void Start () {
		this_Goomba = GetComponent<PE_Obj> ();
	}
	
	// Update is called once per frame
	void Update () {
		this_Goomba.vel.x = x_vel;
	}

	void OnTriggerEnter(Collider other) {

		if(this_Goomba.dir == PE_Dir.downLeft || this_Goomba.dir == PE_Dir.downRight){
			x_vel *= -1;
		}


	}
}
