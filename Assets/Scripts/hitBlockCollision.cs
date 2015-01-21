using UnityEngine;
using System.Collections;

public class hitBlockCollision : MonoBehaviour {

	Animator anim;
	bool wasHit = false;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		wasHit = true;
		anim.SetBool ("wasHit", wasHit);
	}

}
