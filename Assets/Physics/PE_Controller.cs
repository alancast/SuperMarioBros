using UnityEngine;
using System.Collections;

public class PE_Controller : MonoBehaviour {
	private PE_Obj peo;
	
	public Vector3	vel;
	public bool		grounded = false;
	
	public float	hSpeed = 10;
	public float	acceleration = 10;
	public float	jumpVel = 10;
	public float	airSteeringAmt = 1f;
	
	// 0 for no momentum (i.e. 100% drag), 1 for total momentum
	public float	airMomentumX = 1; 
	public float	groundMomentumX = 0.1f;
	
	// Different x & y to limit maximum falling velocity
	public Vector2	maxSpeed = new Vector2( 10, 15 ); 
	
	void Start () {
		peo = GetComponent<PE_Obj>();
	}
	
	// Note that we use Update for input but FixedUpdate for physics. 
	// This is because Unity input is handled based on Update
	void Update () {
		vel = peo.vel; // Pull velocity from the PE_Obj
		grounded = (peo.ground != null);
		
		// Horizontal movement
		// Returns a number [-1..1] ??????????????????????????????????????????????????????
		float vX = Input.GetAxis("Horizontal"); 
		vel.x = vX * hSpeed;
		
		// Jumping with A (which is x or .)
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period)) {
			// Jump if you're grounded
			if (grounded) {
				vel.y = jumpVel;
				// Jumping will set ground = null
				peo.ground = null; 
			}
		}
		
		peo.vel = vel;
	}
}
