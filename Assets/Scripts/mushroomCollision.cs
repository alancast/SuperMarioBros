using UnityEngine;
using System.Collections;

public class mushroomCollision : MonoBehaviour {

	public int shroomState = 0;
	GameObject mario;
	PE_Controller mario_Controller;
	Animator shroomAnim;
	PE_Obj this_mushroom;
	float startTime;
	public float moveTime = .4f;
	public float lifeDuration = 10f;
	public float shroomVel = 2f;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		mario = GameObject.FindGameObjectWithTag ("Player");
		mario_Controller = PE_Controller.instance;
		shroomAnim = GetComponent<Animator>();
		this_mushroom = GetComponent<PE_Obj> ();

		if(mario.transform.position.x > transform.position.x){
			shroomVel *= -1;	
		}
	}

	
	// Update is called once per frame
	void Update () {
		MarioState curState = mario_Controller.state;

		shroomAnim.SetInteger ("state", (int)curState);
		if (curState != MarioState.Big && Time.time - startTime > moveTime) {
			this_mushroom.vel.x = shroomVel;
		}

		if (Time.time - startTime > lifeDuration) {
			PE_Obj thisShroom = this.GetComponent<PE_Obj> ();
			PhysicsEngine.objs.Remove (thisShroom);
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
				
			PE_Controller mario_Control = other.GetComponent<PE_Controller> ();
			if(mario_Control.state == MarioState.Fly){
				CameraMGR.lives++;
				CameraMGR.instance.livesText.text = CameraMGR.lives.ToString(); 
			}
			Vector3 currentPos = other.transform.position;
			currentPos.y += .5f;
			other.transform.position = currentPos;
			int[] states = {(int)(mario_Control.state + 1), 2};
			int newState = Mathf.Min (states);
			mario_Control.state = (MarioState)newState;

						//remove shroom
			PE_Obj thisShroom = this.GetComponent<PE_Obj> ();
			PhysicsEngine.objs.Remove (thisShroom);
			Destroy (this.gameObject);

			Animator anim = other.GetComponent<Animator> ();
			anim.SetInteger ("state", (int)mario_Control.state);
		} else if(this_mushroom.dir == PE_Dir.downLeft || this_mushroom.dir == PE_Dir.downRight){
			if(this_mushroom.vel0.y == 0 && other.tag != "Goomba" && other.tag != "HitBlock" && other.tag != "GoombaCollider"){
				shroomVel *= -1;
			}
		}
	}
}
