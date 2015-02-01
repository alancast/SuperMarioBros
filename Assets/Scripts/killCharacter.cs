using UnityEngine;
using System.Collections;

public class killCharacter : MonoBehaviour {
	public bool		fullKill = false;
	//an invicibility for two seconds
	public float	invicible;

	void OnTriggerEnter(Collider other){
		if (PE_Controller.BeastMode && !fullKill) return;
		if (other.tag == "Player"){
			//fell off map
			if (fullKill){
				CameraMGR.lives -= 1;
				CameraMGR.instance.livesText.text = CameraMGR.lives.ToString();
				Application.LoadLevel("_Scene_Alex_7");
			}
			//hit an enemy
			else{
				//from big to small
				if (PE_Controller.instance.state == MarioState.Big){
					if (Time.time < invicible) return;
					PE_Controller.instance.state = MarioState.Small;
					invicible = Time.time + 2f;
				}
				//from small to dead
				else if (PE_Controller.instance.state == MarioState.Small){
					if (Time.time < invicible) return;
					CameraMGR.lives -= 1;
					CameraMGR.instance.livesText.text = CameraMGR.lives.ToString();
					Application.LoadLevel("_Scene_Alex_7");
				}
				//from fly to big
				else{
					PE_Controller.instance.state = MarioState.Big;
					invicible = Time.time + 2f;
				}


				Animator anim = other.GetComponent<Animator>();
				anim.SetInteger("state", (int) PE_Controller.instance.state);
			}
		}
	}
	
}
