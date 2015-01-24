using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PE_Dir{
	still,
	up,
	down,
	downRight,
	downLeft,
	upLeft,
	upRight
}

public class PhysicsEngine : MonoBehaviour {
	static public List<PE_Obj> 	objs;
	static public float			closeEnough = 0.1f;
	
	public Vector3 		gravity = new Vector3(0, -9.8f, 0);

	void Awake(){
		objs = new List<PE_Obj>();
	}
	
	void FixedUpdate () {
		// Handle the timestep for each object
		float dt = Time.fixedDeltaTime;
		foreach (PE_Obj po in objs) {
			TimeStep(po, dt);
		}

		// Finalize positions
		foreach (PE_Obj po in objs) {
			po.transform.position = po.pos1;
		}
		
	}
	
	
	public void TimeStep(PE_Obj po, float dt) {
		//do nothing if still
		if (po.still) { 
			po.pos0 = po.pos1 = po.transform.position;
			return;
		}
		
		// Velocity
		po.vel0 = po.vel;
		Vector3 tAcc = po.acc;
		//what about if already at -9.8? 
		//you don't want acceleration downward higher than that do you?
		tAcc += gravity;
		po.vel += tAcc * dt;
		
		if (po.vel.x==0) { // Special case when po.vel.x == 0
			if (po.vel.y > 0) {
				po.dir = PE_Dir.up;
			} else {
				po.dir = PE_Dir.down;
			}
		} else if (po.vel.x>0 && po.vel.y>0) {
			po.dir = PE_Dir.upRight;
		} else if (po.vel.x>0 && po.vel.y<=0) {
			po.dir = PE_Dir.downRight;
		} else if (po.vel.x<0 && po.vel.y<=0) {
			po.dir = PE_Dir.downLeft;
		} else if (po.vel.x<0 && po.vel.y>0) {
			po.dir = PE_Dir.upLeft;
		}
		
		// Position
		po.pos1 = po.pos0 = po.transform.position;
		po.pos1 += po.vel * dt;
		
	}
	
	// Static equality functions to deal with floating point math errors
	static public bool EQ(float f0, float f1) {
		if ( Mathf.Abs(f1-f0) <= closeEnough ) {
			return( true );
		}
		return( false );
	}
	
	static public bool LEQ(float f0, float f1) {
		if ( f0 < f1 || Mathf.Abs(f1-f0) <= closeEnough ) {
			return( true );
		}
		return( false );
	}
	
	static public bool GEQ(float f0, float f1) {
		if ( f0 > f1 || Mathf.Abs(f1-f0) <= closeEnough ) {
			return( true );
		}
		return( false );
	}
}
