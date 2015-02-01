using UnityEngine;
using System.Collections;

public class goombaScript : MonoBehaviour {

	public PE_Obj this_Goomba;
	public float marioKillVel = 25f;
	public float x_vel = 1f;
	//public BoxCollider this_collider;
	
	public bool onTop(){
		PE_Obj marioPhys = PE_Controller.instance.GetComponent<PE_Obj> ();

		print (transform.position);
		Vector3 origin = transform.position;
		origin.x += collider.bounds.size.x/2;
		if (marioPhys.vel.y <= 0) {
				if (Physics.Raycast (origin, new Vector3 (0, 1, 0), 
		                    collider.bounds.size.y / 2 + .1f))
								return true;
				origin.x -= collider.bounds.size.x;
				if (Physics.Raycast (origin, new Vector3 (0, 1, 0), 
		                    collider.bounds.size.y / 2 + .1f))
							return true;
				if (Physics.Raycast (transform.position, new Vector3 (0, 1, 0), 
		                    collider.bounds.size.y / 2 + .1f))
							return true;
		}
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
		if (other.tag == "Player" && onTop() ){

			PE_Obj marioPhys = other.GetComponent<PE_Obj> ();
			marioPhys.vel.y = marioKillVel;

			PE_Controller.instance.isJumping = true;
			PE_Controller.instance.stopHeight = marioPhys.transform.position.y + PE_Controller.instance.maxJumpHeight;

			PhysicsEngine.objs.Remove (this_Goomba);
			Destroy(this.gameObject);

			return;
		}

		if (other.tag == "Shell") {
			PhysicsEngine.objs.Remove (this_Goomba);
			Destroy(this.gameObject);
		}
		
		if(this_Goomba.dir == PE_Dir.downLeft || this_Goomba.dir == PE_Dir.downRight){
			if (other.tag != "Player" && other.tag != "Item"){
				this_Goomba.vel = -this_Goomba.vel0;
			}
		}


	}
}
