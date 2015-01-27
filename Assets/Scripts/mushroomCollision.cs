using UnityEngine;
using System.Collections;

public class mushroomCollision : MonoBehaviour {

	public int shroomState = 0;
	GameObject mario;
	PE_Controller mario_Controller;
	Animator shroomAnim;

	// Use this for initialization
	void Start () {
		mario = GameObject.FindGameObjectWithTag ("Player");
		mario_Controller = mario.GetComponent<PE_Controller> ();
		shroomAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		MarioState curState = mario_Controller.state;

		shroomAnim.SetInteger ("state", (int)curState);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {		
			PE_Controller mario_Control = other.GetComponent<PE_Controller> ();
			int[] states = {(int)(mario_Control.state + 1), 2};
			int newState = Mathf.Min(states);
			mario_Control.state = (MarioState)newState;

			//remove shroom
			PE_Obj thisShroom = this.GetComponent<PE_Obj> ();
			PhysicsEngine.objs.Remove (thisShroom);
			Destroy (this.gameObject);

			Animator anim = other.GetComponent<Animator>();
			anim.SetInteger("state", (int) mario_Control.state);
		}
	}
}
