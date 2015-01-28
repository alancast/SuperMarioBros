using UnityEngine;
using System.Collections;

public class goombaScript : MonoBehaviour {

	PE_Obj this_Goomba;
	
	bool onTop(){
		print (transform.position);
		Vector3 origin = transform.position;
		origin.x += transform.collider.bounds.size.x/2;
		if (Physics.Raycast(origin, new Vector3(0, 1, 0), 
		                    transform.collider.bounds.size.y + .1f)) return true;
		origin.x -= transform.collider.bounds.size.x;
		if (Physics.Raycast(origin, new Vector3(0, 1, 0), 
		                    transform.collider.bounds.size.y + .1f)) return true;
		if (Physics.Raycast(transform.position, new Vector3(0, 1, 0), 
		                    	transform.collider.bounds.size.y +.1f)) return true;
		return false;
	}
	
	// Use this for initialization
	void Start () {
		this_Goomba = GetComponent<PE_Obj> ();
		this_Goomba.vel.x = 1;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position, new Vector3(0,1,0), Color.red, .1f);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && onTop()){
			PhysicsEngine.objs.Remove (this_Goomba);
			Destroy(this.gameObject);
			return;
		}
		
		if(this_Goomba.dir == PE_Dir.downLeft || this_Goomba.dir == PE_Dir.downRight){
			this_Goomba.vel = -this_Goomba.vel0;
		}


	}
}
