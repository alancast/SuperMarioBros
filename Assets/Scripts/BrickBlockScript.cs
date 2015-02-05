using UnityEngine;
using System.Collections;

public class BrickBlockScript : MonoBehaviour {
	//the coin that the brickblock will turn to
	public GameObject 	hitItem;

	void OnTriggerEnter(Collider other){
		if (other.tag != "Shell") return;
		PE_Obj this_block = GetComponent<PE_Obj>();
		if (other.transform.position.y > (this_block.transform.position.y + this_block.collider.bounds.size.y/2f)) return;
		PhysicsEngine.objs.Remove (this_block);
		Destroy(this.gameObject);
	}
	
	void Update(){
		if (!PE_Controller.instance.powed) return;
		Instantiate(hitItem, this.transform.position, Quaternion.identity);
		PE_Obj this_block = GetComponent<PE_Obj>();
		PhysicsEngine.objs.Remove (this_block);
		Destroy(this.gameObject);
	}
	
	
}
