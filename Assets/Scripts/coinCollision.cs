using UnityEngine;
using System.Collections;

public class coinCollision : MonoBehaviour {


	void OnTriggerEnter(Collider other) {

		PE_Obj thisCoin = this.GetComponent<PE_Obj> ();

		PhysicsEngine.objs.Remove(thisCoin);

		//PhysicsEngine.objs.RemoveAt (coinIndex);

		Destroy (this.gameObject);
		
		//update GUI
		CameraMGR.instance.score += 100;
		CameraMGR.instance.scoreText.text = CameraMGR.instance.score.ToString();
		CameraMGR.instance.coinage += 1;
		CameraMGR.instance.coinageText.text = CameraMGR.instance.coinage.ToString();
	}

		

}
