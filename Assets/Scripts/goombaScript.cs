﻿using UnityEngine;
using System.Collections;

public class goombaScript : MonoBehaviour {

	public PE_Obj this_Goomba;
	public float marioKillVel = 25f;
	public float x_vel = 1f;
	//public BoxCollider this_collider;
	
<<<<<<< HEAD
	public bool onTop(){
		print (transform.position);
=======
	bool onTop(){
>>>>>>> Alex
		Vector3 origin = transform.position;
		origin.x += collider.bounds.size.x/2;
		if (Physics.Raycast(origin, new Vector3(0, 1, 0), 
		                    collider.bounds.size.y + .1f)) return true;
		origin.x -= collider.bounds.size.x;
		if (Physics.Raycast(origin, new Vector3(0, 1, 0), 
		                    collider.bounds.size.y + .1f)) return true;
		if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), 
		                    	collider.bounds.size.y + .1f)) return true;
		return false;
	}
	
	// Use this for initialization
	void Start () {
		this_Goomba = GetComponent<PE_Obj> ();
		this_Goomba.vel.x = x_vel;

	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position, new Vector3(0,1,0), Color.red, .1f);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && onTop()){

			PE_Obj marioPhys = other.GetComponent<PE_Obj> ();
			marioPhys.vel.y = marioKillVel;

			PhysicsEngine.objs.Remove (this_Goomba);
			Destroy(this.gameObject);

			return;
		}
		
		if(this_Goomba.dir == PE_Dir.downLeft || this_Goomba.dir == PE_Dir.downRight){
			if (other.tag != "Player"){
				this_Goomba.vel = -this_Goomba.vel0;
			}
		}


	}
}
