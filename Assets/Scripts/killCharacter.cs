using UnityEngine;
using System.Collections;

public class killCharacter : MonoBehaviour {
	public bool		fullKill = false;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Goomba" && fullKill) {
			koopaScript otherEnemy = other.GetComponent<koopaScript> ();
			if(otherEnemy && otherEnemy.state == KoopaState.Winged){
				other.transform.position = otherEnemy.dest;
			}
		}

		if (PE_Controller.BeastMode && !fullKill) return;
		if (other.tag == "Player"){
			//fell off map
			if (fullKill){
				CameraMGR.lives -= 1;
				CameraMGR.instance.livesText.text = CameraMGR.lives.ToString();
				if (CameraMGR.lives <= 0){
					CameraMGR.lives = 4;
					CameraMGR.score = 0;
					CameraMGR.coinage = 0;
					Application.LoadLevel("_Scene_End_Game");
				}
				else {
					if (PE_Controller.instance.isAlpha){
						Application.LoadLevel("_Scene_Alpha_3");
					}
					else{
						Application.LoadLevel("_Scene_Alex_7");
					}
				}
			}
			//hit an enemy
			else{
				//from big to small
				if (PE_Controller.instance.state == MarioState.Big){
					if (Time.time < PE_Controller.invincible) return;
					PE_Controller.instance.state = MarioState.Small;
					PE_Controller.invincible = Time.time + 2f;
					PE_Controller.instance.source.PlayOneShot(PE_Controller.instance.shrink);
				}
				//from small to dead
				else if (PE_Controller.instance.state == MarioState.Small){
					if (Time.time < PE_Controller.invincible) return;
					CameraMGR.lives -= 1;
					CameraMGR.instance.livesText.text = CameraMGR.lives.ToString();
					if (CameraMGR.lives <= 0){
						CameraMGR.lives = 4;
						CameraMGR.score = 0;
						CameraMGR.coinage = 0;
						Application.LoadLevel("_Scene_End_Game");
					}
					else {
						if (PE_Controller.instance.isAlpha){
							Application.LoadLevel("_Scene_Alpha_3");
						}
						else{
							Application.LoadLevel("_Scene_Alex_7");
						}
					}
				}
				//from fly to big
				else{
					PE_Controller.instance.state = MarioState.Big;
					PE_Controller.invincible = Time.time + 2f;
					PE_Controller.instance.source.PlayOneShot(PE_Controller.instance.shrink);
				}


				Animator anim = other.GetComponent<Animator>();
				anim.SetInteger("state", (int) PE_Controller.instance.state);
			}
		}
	}
	
}
