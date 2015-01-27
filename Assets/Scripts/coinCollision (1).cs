using UnityEngine;
using System.Collections;

public class coinCollision : MonoBehaviour {


	void OnTriggerEnter(Collider other) {

		PE_Obj thisCoin = this.GetComponent<PE_Obj> ();

		PhysicsEngine.objs.Remove(thisCoin);

		//PhysicsEngine.objs.RemoveAt (coinIndex);

		Destroy (this.gameObject);
	}

		

}
