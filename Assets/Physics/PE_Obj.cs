using UnityEngine;
using System.Collections;



public class PE_Obj : MonoBehaviour {
	public bool			still = false;
	
	public Vector3		acc = Vector3.zero;
	
	public Vector3		vel = Vector3.zero;
	public Vector3		vel0 = Vector3.zero;
	
	// current position
	public Vector3		pos0 = Vector3.zero;
	// next position
	public Vector3		pos1 = Vector3.zero;
	
	public PE_Dir		dir = PE_Dir.still;
	
	// Stores whether this is on the ground
	public PE_Obj		ground = null; 
	
	
	void Start() {
		if (PhysicsEngine.objs.IndexOf(this) == -1) {
			PhysicsEngine.objs.Add(this);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		// Ignore collisions of still objects
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		
		ResolveCollisionWith(otherPEO);
	}
	
	void OnTriggerStay(Collider other) {
		OnTriggerEnter(other);
	}
	
	void OnTriggerExit(Collider other) {
		// Ignore collisions of still objects
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		
		// This sets ground to null if we fall off of the current ground
		// Jumping will also set ground to null
		if (ground == otherPEO) {
			ground = null;
		}
	}
	
	void ResolveCollisionWith(PE_Obj that) {
		// Assumes that "that" is still
		Vector3 posFinal = pos1; // Sets a defaut value for posFinal
		
		// AABB / AABB collision
		// Axis-Aligned Bounding Box
		// With AABB collisions, we're usually concerned with corners and deciding which corner to consider when making comparisons.
		// I believe that this corner should be determined by looking at the velocity of the moving body (this one)
		
		Vector3 a0, a1, b; // a0-moving corner last frame, a1-moving corner now, b-comparison corner on other object
		a0 = a1 = b = Vector3.zero;	 // Sets a default value to keep the compiler from complaining
		Vector3 delta = pos1 - pos0;
		
		if (dir == PE_Dir.down) {
			// Just resolve to be on top
			a1 = pos1;
			b = that.pos1;
			if(a1.y < b.y){
				transform.position = pos1 = posFinal;
				return;
			}
			a1.y -= transform.lossyScale.y/2f;
			b.y += that.transform.lossyScale.y/2f;
			if (b.y > a1.y) {
				posFinal.y += Mathf.Abs( a1.y - b.y );
			}
			// Handle vel
			vel.y = 0;
			
			if (ground == null) ground = that;
			transform.position = pos1 = posFinal;
			return;
		}
		
		if (dir == PE_Dir.up) {
			// Just resolve to be below
			a1 = pos1;
			a1.y += transform.lossyScale.y/2f;
			b = that.pos1;
			b.y -= that.transform.lossyScale.y/2f;
//			if (b.y < a1.y) {
//				posFinal.y -= Mathf.Abs( a1.y - b.y );
//			}
			// Handle vel
			vel.y = 0;
			
			transform.position = pos1 = posFinal;
			return;
		}
		
		if (dir == PE_Dir.upRight) { // Bottom, Left is the comparison corner
			a1 = pos1;
			a1.x += transform.lossyScale.x/2f;
			a1.y += transform.lossyScale.y/2f;
			a0 = a1 - delta;
			b = that.pos1;
			b.x -= that.transform.lossyScale.x/2f;
			b.y -= that.transform.localScale.y/2f;
		}
		
		if (dir == PE_Dir.upLeft) { // Bottom, Right is the comparison corner
			a1 = pos1;
			a1.x -= transform.lossyScale.x/2f;
			a1.y += transform.lossyScale.y/2f;
			a0 = a1 - delta;
			b = that.pos1;
			b.x += that.transform.lossyScale.x/2f;
			b.y -= that.transform.localScale.y/2f;
		}
		
		if (dir == PE_Dir.downLeft) { // Top, Right is the comparison corner
			a1 = pos1;
			a1.x -= transform.lossyScale.x/2f;
			a1.y -= transform.lossyScale.y/2f;
			a0 = a1 - delta;
			b = that.pos1;
			b.x += that.transform.lossyScale.x/2f;
			b.y += that.transform.localScale.y/2f;
		}
		
		if (dir == PE_Dir.downRight) { // Top, Left is the comparison corner
			a1 = pos1;
			a1.x += transform.lossyScale.x/2f;
			a1.y -= transform.lossyScale.y/2f;
			a0 = a1 - delta;
			b = that.pos1;
			b.x -= that.transform.lossyScale.x/2f;
			b.y += that.transform.localScale.y/2f;
		}
		
		// In the x dimension, find how far along the line segment between a0 and a1 we need to go to encounter b
		float u = (b.x - a0.x) / (a1.x - a0.x);
		
		// Determine this point using linear interpolation (see the appendix of the book)
		Vector3 pU = (1-u)*a0 + u*a1;
		
		// Use pU.y vs. b.y to tell which side of PE_Obj "that" PE_Obj "this" should be on
		switch (dir) {
		case PE_Dir.upRight:
			if (pU.y > b.y) { // hit the left side
				posFinal.x -= Mathf.Abs(a1.x - b.x);
				
				// Handle vel
				vel.x = 0;
				
			} else { // hit the bottom
				posFinal.y -= Mathf.Abs(a1.y - b.y);
				
				// Handle vel
				vel.y = 0;
				
			}
			break;
			
		case PE_Dir.downRight:
			if (pU.y < b.y) { // hit the left side
				posFinal.x -= Mathf.Abs(a1.x - b.x);
				
				// Handle vel
				vel.x = 0;
				
			} else { // hit the top
				posFinal.y += Mathf.Abs(a1.y - b.y);
				
				// Handle vel
				vel.y = 0;
				
				if (ground == null) ground = that;
			}
			break;
			
		case PE_Dir.upLeft:
			if (pU.y > b.y) { // hit the right side
				posFinal.x += Mathf.Abs(a1.x - b.x);
				
				// Handle vel
				vel.x = 0;
				
			} else { // hit the bottom
				posFinal.y -= Mathf.Abs(a1.y - b.y);
				
				// Handle vel
				vel.y = 0;
				
			}
			break;
			
		case PE_Dir.downLeft:
			if (pU.y < b.y) { // hit the right side
				posFinal.x += Mathf.Abs(a1.x - b.x);
				
				// Handle vel
				vel.x = 0;
				
			} else { // hit the top
				posFinal.y += Mathf.Abs(b.y - a1.y);
				
				// Handle vel
				vel.y = 0;
				
				if (ground == null) ground = that;
			}
			break;
		}
		
		transform.position = pos1 = posFinal;
	}
	
	
}

