using UnityEngine;
using System.Collections;

public class FireVelController : MonoBehaviour {
	PE_Obj this_FireBall;
	public Vector3 fireSpeed;
	float startTime;
	public float lifeTime = .08f;


	// Use this for initialization
	void Start () {
		this_FireBall = GetComponent<PE_Obj> ();
		//this_FireBall.acc = Vector3.zero;
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		//this_FireBall.acc = Vector3.zero;
		this_FireBall.vel = fireSpeed;

		if((startTime - Time.time) > lifeTime){

			PE_Obj thisFB = this.GetComponent<PE_Obj> ();
			PhysicsEngine.objs.Remove(thisFB);
			Destroy (this.gameObject);
		}
	}
}
