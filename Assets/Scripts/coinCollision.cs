﻿using UnityEngine;
using System.Collections;

public class coinCollision : MonoBehaviour {

	public bool 		isTemporary = false;
	float 				startTime;
	float		coinLife = .4f;
	//public AudioClip sound;
	AudioSource source;
	public float volume = 1f;
	float deathTime = -1f;
	
	void Start(){
		startTime = Time.time;
		if(this.CompareTag("Item")){
			isTemporary = true;
		}
	}

	void Awake(){
		source = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag != "Player") return;
		source.Play ();

		CashThisCoin ();
	}

	void Update(){
		if (isTemporary && ((Time.time - startTime) > coinLife)) {
			CashThisCoin();
		}
	}
	

	void CashThisCoin(){
		
		PE_Obj thisCoin = this.GetComponent<PE_Obj> ();
		
		PhysicsEngine.objs.Remove(thisCoin);
		
		//PhysicsEngine.objs.RemoveAt (coinIndex);
		
		Destroy (this.gameObject);
		
		//update GUI
		if (isTemporary) {
			CameraMGR.score += 100;
		} else {
			CameraMGR.score += 50;
		}
		CameraMGR.instance.scoreText.text = CameraMGR.score.ToString();
		CameraMGR.coinage += 1;
		CameraMGR.instance.coinageText.text = CameraMGR.coinage.ToString();

	}

		

}
