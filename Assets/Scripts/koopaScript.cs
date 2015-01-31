using UnityEngine;
using System.Collections;

public enum KoopaState{
	MovingShell,
	StillShell,
	Koopa
}

public class koopaScript : goombaScript {

	public KoopaState	state = KoopaState.Koopa;
	public float 		shellVel = 18f;

	// Update is called once per frame
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && this.GetComponent<koopaScript> ().onTop()){
			PE_Obj marioPhys = other.GetComponent<PE_Obj> ();
			marioPhys.vel.y = marioKillVel;

			if (this.state == KoopaState.Koopa){
				this.state = KoopaState.StillShell;
				this_Goomba.vel = Vector3.zero;
			}
			else if (this.state == KoopaState.StillShell){
				this.state = KoopaState.MovingShell;
			}
			else if (this.state == KoopaState.MovingShell){
				this.state = KoopaState.StillShell;
			}


		}



		if(this_Goomba.dir == PE_Dir.downLeft || this_Goomba.dir == PE_Dir.downRight){
			if (other.tag != "Player"){
				this_Goomba.vel = -this_Goomba.vel0;
			}
		}
		
		
	}
}
