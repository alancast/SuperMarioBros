using UnityEngine;
using System.Collections;

public enum KoopaState{
	MovingShell,
	StillShell,
	Koopa,
	Winged
}

public class koopaScript : goombaScript {

	public KoopaState	state = KoopaState.Winged;
	//public float 		shellVel = 18f;
	Animator koopaAnim;
	bool isShell = false;
	public Transform killZone;
	BoxCollider koopaCollider;
	Vector3 shellColliderDim = new Vector3(.7f, .9f, .2f);
	Vector3 startKillZonePos = Vector3.zero;
	Vector3 shellKillZoneSize = new Vector3(.66f, .3f, 1f);
	public float shellSpeed = 15f;
	bool isWinged = false;
	public float koopaJumpVel = 14f;
	public Vector3 dest = new Vector3(85f, 3.4f, 0f);

	void Start () {
		this_Goomba = GetComponent<PE_Obj> ();
		this_Goomba.vel.x = x_vel;

		koopaAnim = GetComponent<Animator> ();
		koopaCollider = GetComponent<BoxCollider> ();
		startKillZonePos = transform.GetChild (0).position;
		koopaAnim.SetInteger("state", (int) this.state );
	}

	void Update()
	{
		if (this.state == KoopaState.Winged) {
			if (this_Goomba.ground) {
					this_Goomba.vel.y = koopaJumpVel;
			}
		}

		if (Mathf.Abs (this_Goomba.vel.x) < .1 && this.tag == "Goomba") {
			this_Goomba.vel.x = -1*x_vel;
		}
	}

	// Update is called once per frame
	void OnTriggerEnter(Collider other) {

		if (other.tag == "Shell") {
			PhysicsEngine.objs.Remove (this_Goomba);
			Destroy(this.gameObject);
		}

		if (other.tag == "Player" && this.state == KoopaState.StillShell) {

			this_Goomba.vel.x = shellSpeed;

			if(this_Goomba.transform.position.x < other.transform.position.x){
				this_Goomba.vel.x = -shellSpeed;
			}

			this.state = KoopaState.MovingShell;
			killZone = transform.GetChild(0);
			killZone.position = transform.position;
		}

		else if (other.tag == "Player" && this.GetComponent<koopaScript> ().onTop()){

			PE_Obj marioPhys = other.GetComponent<PE_Obj> ();
			marioPhys.vel.y = marioKillVel;

			PE_Controller.instance.isJumping = true;
			PE_Controller.instance.stopHeight = marioPhys.transform.position.y + PE_Controller.instance.maxJumpHeight;


			if (this.state == KoopaState.Winged){
				this.state = KoopaState.Koopa;
				isWinged = false;

				koopaAnim.SetInteger("state", (int) this.state );
			}
			else if (this.state == KoopaState.Koopa){
				this.state = KoopaState.StillShell;
				this_Goomba.vel = Vector3.zero;

				isShell = true;

				koopaAnim.SetBool("isShell", isShell);
				koopaCollider.size = shellColliderDim;
				killZone = transform.GetChild(0);
				Vector3 tmp = startKillZonePos;
				tmp.y = -200f;
				killZone.position = tmp;
				killZone.localScale = shellKillZoneSize;
				tag = "Shell";

			}
			else if (this.state == KoopaState.MovingShell){
				Vector3 tmp = startKillZonePos;
				tmp.y = -200f;
				killZone.position = tmp;
				this.state = KoopaState.StillShell;
				this_Goomba.vel = Vector3.zero;
			}


		}


		if(this_Goomba.dir == PE_Dir.downLeft || this_Goomba.dir == PE_Dir.downRight){
			if (other.tag != "Player" && this_Goomba.vel0.y > -.1 && other.tag != "Goomba" && other.tag != "Platform" && other.tag != "Item"){

				this_Goomba.vel.x = -1*this_Goomba.vel0.x;
			}
		}
		
		
	}
}
