using UnityEngine;
using System.Collections;

public class mushroomCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {		
			PE_Obj thisShroom = this.GetComponent<PE_Obj> ();
		
			PhysicsEngine.objs.Remove (thisShroom);
		
			Destroy (this.gameObject);
		}
	}
}
