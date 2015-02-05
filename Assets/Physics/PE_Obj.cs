﻿using UnityEngine;
using System.Collections;



public class PE_Obj : MonoBehaviour {
	public bool			still = false;
	
	public Vector3		acc = Vector3.zero;
	
	public Vector3		vel = Vector3.zero;
	public Vector3		vel0 = Vector3.zero;
	// Velocity based on relative position from last frame to this
	public Vector3		velRel = Vector3.zero; 
	
	// current position
	public Vector3		_pos0 = Vector3.zero;
	// next position
	public Vector3		_pos1 = Vector3.zero;
	
	public PE_Dir		dir = PE_Dir.still;
	
	// Stores whether this is on the ground
	public PE_Obj		ground = null; 
	
	//
	public float 		raycastDistance;
	
	public Vector3		pos0 {
		get { return( _pos0); }
		set {
			float d = (value - _pos0).magnitude;
			if (d > 1 && gameObject.name == "Mario") {
				Debug.Log ("Big change in pos0!");
			}
			_pos0 = value;
		}
	}
	
	public Vector3		pos1 {
		get { return( _pos1); }
		set {
			float d = (value - _pos1).magnitude;
			if (d > 1 && gameObject.name == "Mario") {
				Debug.Log ("Big change in pos1!");
			}
			_pos1 = value;
		}
	}
	
	bool onTop(){
		return Physics.Raycast(transform.position, new Vector3(0, -1, 0), 
		                    collider.bounds.size.y/2 + raycastDistance);
	}
	
	bool upAndAroundLeft(){
		//should return true if right raycast is under block but middle isn't
		Vector3 origin = transform.position;
		origin.x += transform.collider.bounds.size.x/2f;
		//if right isn't under block
		if (!Physics.Raycast(origin, new Vector3(0, 1, 0), 
		                    transform.collider.bounds.size.y + .1f)){
			return false;
		}
		//if middle isn't under block
		if (!Physics.Raycast(transform.position, new Vector3(0, 1, 0), 
		                    transform.collider.bounds.size.y +.1f)){
			return true;                    
		}
		return false;
	}
	bool upAndAroundRight(){
		//should return true if left raycast is under block but middle isn't
		Vector3 origin = transform.position;
		origin.x -= transform.collider.bounds.size.x/2f;
		//if left isn't under block
		if (!Physics.Raycast(origin, new Vector3(0, 1, 0), 
		                     transform.collider.bounds.size.y + .1f)) return false;
		//if middle isn't under block
		if (!Physics.Raycast(transform.position, new Vector3(0, 1, 0), 
		                     transform.collider.bounds.size.y +.1f)) return true;
		return false;
	}
	
	
	void Start() {
		if (PhysicsEngine.objs.IndexOf(this) == -1) {
			_pos1 = _pos0 = transform.position;
			PhysicsEngine.objs.Add(this);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		// Ignore collisions of still objects
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		if (other.tag == "Item" && tag == "Player") return;
		if (other.tag == "Coin") return;
		if (other.tag == "Goomba") return;
		if (tag == "Player" && other.tag == "GoombaCollider") return;
		if ((other.tag == "GoombaCollider" && tag != "Goomba") ||
		    (tag == "GoombaCollider" && other.tag != "Goomba")) return;
		if ((other.tag == "HitBlock" && tag == "Goomba" && !PE_Controller.instance.isAlpha) ||
		    (tag == "HitBlock" && other.tag == "Goomba" && !PE_Controller.instance.isAlpha)) return;
		if ((other.tag == "Goomba" && tag == "Goomba") ||
		    (tag == "Goomba" && other.tag == "Goomba")) return;
		if (tag == "Goomba" && other.tag == "Player") return;
		if ((tag == "Shell" && other.tag == "Goomba") ||
			(other.tag == "Shell" && tag == "Goomba")) return;
		if ((tag == "Shell" && other.tag == "Player") ||
		    (other.tag == "Shell" && tag == "Player")) return;
		if ((tag == "Item" && other.tag == "Goomba") ||
		    (other.tag == "Item" && tag == "Goomba")) return;
		if (tag == "Shell" && other.tag == "HitBlock"){
			hitBlockCollision block_instance = otherPEO.GetComponent<hitBlockCollision>();
			block_instance.blockHit();
		}

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
	
	void OnDrawGizmos() {
		if (vel.magnitude != 0) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(a0,0.2f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(a0,a1);
			Gizmos.DrawWireSphere(a1,0.2f);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(a1,posFinal);
			Gizmos.DrawWireSphere(posFinal,0.2f);
		}
	}
	
	// a0-moving corner last frame, a1-moving corner now, b-comparison corner on other object
	public Vector3 a0, a1, b, delta, pU, posFinal; 
	
	void ResolveCollisionWith(PE_Obj that) {
		// Assumes that "that" is still
		// Sets a defaut value for posFinal
		posFinal = pos1; 
		
		// AABB / AABB collision
		// Axis-Aligned Bounding Box
		// With AABB collisions, we're usually concerned with corners and deciding which corner to consider when making comparisons.
		// I believe that this corner should be determined by looking at the velocity of the moving body (this one)
		
		a0 = a1 = b = Vector3.zero;	 // Sets a default value to keep the compiler from complaining
		delta = pos1 - pos0;
		
		if (dir == PE_Dir.down) {
			// Just resolve to be on top
			a1 = pos1;
			b = that.pos1;
			a1.y -= collider.bounds.size.y/2f;
			a0 = a1 - delta;
			b.y += that.collider.bounds.size.y/2f;
			if (PhysicsEngine.GEQ( a0.y, b.y ) && b.y > a1.y) {
				posFinal.y += Mathf.Abs( a1.y - b.y );
				// Handle vel
				vel.y = 0;
				
				if (ground == null) ground = that;
				if (PE_Controller.instance.isFlying && tag == "Player"){
					PE_Controller.instance.isFlying = false;
				}
			}
		}
		
		if (dir == PE_Dir.up) {
			// Just resolve to be below
			a1 = pos1;
			a1.y += collider.bounds.size.y/2f;
			if(that.tag == "Platform"){
				transform.position = pos1 = posFinal;
				return;
			}
			a0 = a1 - delta;
			b = that.pos1;
			b.y -= that.collider.bounds.size.y/2f;
			//hang to the ceiling
			if (that.tag == "Alpha"){
				float yOff = Mathf.Abs( a1.y - b.y );
				if (yOff < .4f){
					posFinal.y -= Mathf.Abs( a1.y - b.y );
				}
				transform.position = pos1 = posFinal;
				vel.y = 0;
				return;
			}
			//to go up and around block
			if (upAndAroundLeft()){
				float upOffsetX = Mathf.Abs((b.x - that.collider.bounds.size.x/2f) - (a1.x + collider.bounds.size.x/2f) - .2f);
				posFinal.x -= upOffsetX;
				// Handle vel
				vel.x = 0;
				print ("left");
			}
			else if (upAndAroundRight()){
				float upOffsetX = Mathf.Abs((b.x + that.collider.bounds.size.x/2f) - (a1.x - collider.bounds.size.x/2f) + .2f);
				posFinal.x += upOffsetX;
				// Handle vel
				vel.x = 0;
				print ("right");
			}
			//underneath block
			else if ( PhysicsEngine.LEQ( a0.y, b.y ) && b.y <= a1.y) {
				posFinal.y -= Mathf.Abs( a1.y - b.y );
				// Handle vel
				vel.y = 0;
				PE_Controller.instance.isJumping = false;
				if (that.tag == "HitBlock"){
					if(Time.time > PE_Controller.instance.cantHitTil){
						hitBlockCollision block_instance = that.GetComponent<hitBlockCollision>();
						block_instance.blockHit();
					}
				}
			}
		}
		
		if (dir == PE_Dir.upRight) { // Bottom, Left is the comparison corner
			a1 = pos1;
			a1.x += collider.bounds.size.x/2f;
			a1.y += collider.bounds.size.y/2f;
			if(that.tag == "Platform"){
				transform.position = pos1 = posFinal;
				return;
			}
			a0 = a1 - delta;
			b = that.pos1;
			b.x -= that.collider.bounds.size.x/2f;
			b.y -= that.collider.bounds.size.y/2f;
			//underneath the alpha
			if (that.tag == "Alpha" && a1.y < (b.y +.4)){
				posFinal.y -= Mathf.Abs( a1.y - b.y );
				transform.position = pos1 = posFinal;
				vel.y = 0;
				PE_Controller.instance.stopHeight = transform.position.y + PE_Controller.instance.maxJumpHeight;
				return;
			}
			//underneath object
			if (a1.y < (b.y +.4)){
				posFinal.y = b.y - that.collider.bounds.size.y/2f - collider.bounds.size.y/2f;
				transform.position = pos1 = posFinal;
				vel.y = 0;
				PE_Controller.instance.isJumping = false;
				if (that.tag == "HitBlock"){
					if(Time.time > PE_Controller.instance.cantHitTil){
						hitBlockCollision block_instance = that.GetComponent<hitBlockCollision>();
						block_instance.blockHit();
					}
				}
				return;
			}
		}
		
		if (dir == PE_Dir.upLeft) { // Bottom, Right is the comparison corner
			a1 = pos1;
			a1.x -= collider.bounds.size.x/2f;
			a1.y += collider.bounds.size.y/2f;
			if(that.tag == "Platform"){
				transform.position = pos1 = posFinal;
				return;
			}
			a0 = a1 - delta;
			b = that.pos1;
			b.x += that.collider.bounds.size.x/2f;
			b.y -= that.collider.bounds.size.y/2f;
			//underneath the alpha
			if (that.tag == "Alpha" && a1.y < (b.y +.4)){
				posFinal.y -= Mathf.Abs( a1.y - b.y );
				transform.position = pos1 = posFinal;
				vel.y = 0;
				PE_Controller.instance.stopHeight = transform.position.y + PE_Controller.instance.maxJumpHeight;
				return;
			}
			//underneath object
			if (a1.y < (b.y +.4)){
				posFinal.y = b.y - that.collider.bounds.size.y/2f - collider.bounds.size.y/2f;
				transform.position = pos1 = posFinal;
				vel.y = 0;
				PE_Controller.instance.isJumping = false;
				if (that.tag == "HitBlock"){
					if(Time.time > PE_Controller.instance.cantHitTil){
						hitBlockCollision block_instance = that.GetComponent<hitBlockCollision>();
						block_instance.blockHit();
					}
				}
				return;
			}
		}
		
		if (dir == PE_Dir.downLeft) { // Top, Right is the comparison corner
			a1 = pos1;
			a1.x -= collider.bounds.size.x/2f;
			a1.y -= collider.bounds.size.y/2f;
			a0 = a1 - delta;
			b = that.pos1;
			b.x += that.collider.bounds.size.x/2f;
			b.y += that.collider.bounds.size.y/2f;
			if(!onTop() && that.tag == "Platform"){
				transform.position = pos1 = posFinal;
				return;
			}
		}
		
		if (dir == PE_Dir.downRight) { // Top, Left is the comparison corner
			a1 = pos1;
			a1.x += collider.bounds.size.x/2f;
			a1.y -= collider.bounds.size.y/2f;
			a0 = a1 - delta;
			b = that.pos1;
			b.x -= that.collider.bounds.size.x/2f;
			b.y += that.collider.bounds.size.y/2f;
			if(!onTop() && that.tag == "Platform"){
				transform.position = pos1 = posFinal;
				return;
			}
		}
		
		// In the x dimension, find how far along the line segment between a0 and a1 we need to go to encounter b
		float u = (b.x - a0.x) / (a1.x - a0.x);
		
		// Determine this point using linear interpolation (see the appendix of the book)
		Vector3 pU = (1-u)*a0 + u*a1;
		
		// Find distance we would have to offset in x or y
		float offsetX = Mathf.Abs(a1.x - b.x);
		float offsetY = Mathf.Abs(a1.y - b.y);
		
		// Use pU.y vs. b.y to tell which side of PE_Obj "that" PE_Obj "this" should be on
		switch (dir) {
		case PE_Dir.upRight:
			if (pU.y > b.y || u == 0) { // hit the left side
				posFinal.x -= offsetX;
				
				// Handle vel
				vel.x = 0;	
				if (tag == "Player"){
					dir = PE_Dir.up;
				}			
			}
			break;
			
		case PE_Dir.downRight:
			if (pU.y < (b.y -.1f) || u == 0) { // hit the left side
				if (offsetX < .7f){
					posFinal.x -= offsetX;
				}
				
				// Handle vel
				vel.x = 0;
				if (tag == "Player"){
					dir = PE_Dir.down;
				}
				
			} else { // hit the top
				posFinal.y += offsetY;
				
				// Handle vel
				vel.y = 0;
				
				if (ground == null) ground = that;
				if (PE_Controller.instance.isFlying && tag == "Player"){
					PE_Controller.instance.isFlying = false;
				}
			}
			break;
			
		case PE_Dir.upLeft:
			if (pU.y > b.y || u == 0) { // hit the right side
				posFinal.x += offsetX;
				
				// Handle vel
				vel.x = 0;
				if (tag == "Player"){
					dir = PE_Dir.up;
				}
				
			}
			break;
			
		case PE_Dir.downLeft:
			if (pU.y < (b.y -.1f) || u == 0) { // hit the right side
				if (offsetX < .7f){
					posFinal.x += offsetX;
				}
				
				// Handle vel
				vel.x = 0;
				if (tag == "Player"){
					dir = PE_Dir.down;
				}
			} else { // hit the top
				posFinal.y += offsetY;
				
				// Handle vel
				vel.y = 0;
				
				if (ground == null) ground = that;
				if (PE_Controller.instance.isFlying && tag == "Player"){
					PE_Controller.instance.isFlying = false;
				}
			}
			break;
		}
		
		transform.position = pos1 = posFinal;
	}
	
	
}

