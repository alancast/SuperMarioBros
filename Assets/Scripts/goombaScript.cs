using UnityEngine;
using System.Collections;

public enum GoombaState{
	Goomba,
	Winged
}

public class goombaScript : MonoBehaviour {

	public PE_Obj this_Goomba;
	public float marioKillVel = 25f;
	public float x_vel = -2f;
	public GoombaState goombaState = GoombaState.Winged;
	Animator goombaAnim;
	float startTime;
	public float jumpVel = 12f;
	public float timeBetweenJumps = .8f;
	//public BoxCollider this_collider;
	
	public bool onTop(){
		PE_Obj marioPhys = PE_Controller.instance.GetComponent<PE_Obj> ();

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
		startTime = Time.time;
		this_Goomba = GetComponent<PE_Obj> ();
		this_Goomba.vel.x = x_vel;
		goombaAnim = GetComponent<Animator> ();
		goombaAnim.SetInteger("state", (int) this.goombaState);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position, new Vector3(0,1,0), Color.red, .1f);

		if (this.goombaState == GoombaState.Winged) {
			if(Time.time - startTime > timeBetweenJumps){
				startTime = Time.time;
				this_Goomba.vel.y = jumpVel;
			}
		}

		if (Mathf.Abs (this_Goomba.vel.x) < .1) {
			this_Goomba.vel.x = x_vel;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && onTop() ){

			PE_Obj marioPhys = other.GetComponent<PE_Obj> ();
			marioPhys.vel.y = marioKillVel;

			PE_Controller.instance.isJumping = true;
			PE_Controller.instance.justKilled = true;
			PE_Controller.instance.stopHeight = marioPhys.transform.position.y + PE_Controller.instance.maxJumpHeight;


			if(this.goombaState == GoombaState.Goomba){
				PhysicsEngine.objs.Remove (this_Goomba);
				PE_Controller.instance.source.PlayOneShot(PE_Controller.instance.kill);
				Destroy(this.gameObject);
			}
			else if(this.goombaState == GoombaState.Winged){
				this.goombaState = GoombaState.Goomba;

				goombaAnim.SetInteger("state", (int) this.goombaState);
				PE_Controller.instance.source.PlayOneShot(PE_Controller.instance.kill);
			}

			return;
		}

		if (other.tag == "Shell" || other.tag == "Tail") {
			PhysicsEngine.objs.Remove (this_Goomba);
			if (other.tag == "Tail"){
				PE_Controller.instance.source.PlayOneShot(PE_Controller.instance.kill);
			}
			Destroy(this.gameObject);
		}
		
		if(this_Goomba.dir == PE_Dir.downLeft || this_Goomba.dir == PE_Dir.downRight){
			if (other.tag != "Player" && other.tag != "Item" && this_Goomba.vel0.y > -.1
				&& other.tag != "Goomba"){
				this_Goomba.vel.x = -1*this_Goomba.vel0.x;
			}

		}

		if(this.goombaState == GoombaState.Winged && other.tag == "GoombaCollider"){
			this_Goomba.vel.x = -1*this_Goomba.vel0.x;
		}
		
		
	}
}
