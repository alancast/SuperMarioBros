using UnityEngine;
using System.Collections;

public enum MarioState{
	Small,
	Big,
	Fly
}

public class PE_Controller : MonoBehaviour {
	public static PE_Controller instance;
	private PE_Obj peo;
	public MarioState	state = MarioState.Small;
	
	public Vector3	vel;
	public bool		grounded = false;
	//so that you can continue jumping
	public bool 	isJumping = false;
	//to change the max speed for sprinting
	public bool 	isSprinting = false;
	//slowing down to normal run speed
	public bool 	slowingDown = false;
	public bool		isFlying = false;
	
	public float	acceleration = 10;
	public float 	accel_speed = 10;
	public float	jumpVel = 10;
	public float 	maxJumpHeight = 3;
	//will get set each jump to the max height to stop at
	public float 	stopHeight = 0;
	
	//0 for never stopping, 1 for stopping faster
	public float 	slowDownFactor = 5;
	//0 for none, 1 for infinite
	public float	groundMomentumX = 0.9f;
	//if abs x velocity is less than this kill it
	public float	killVelThreshold = .5f;

	// Different x & y to limit maximum falling velocity
	public Vector2	maxSpeed = new Vector2( 8, 15 ); 
	public float 	maxSprintX = 16;
	
	void Awake(){
		instance = this;
	}
	
	void Start () {
		peo = GetComponent<PE_Obj>();
	}
	
	void Update () {
		grounded = (peo.ground != null);
		
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			acceleration = -accel_speed;
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			acceleration = accel_speed;
		if (!Input.GetKey(KeyCode.RightArrow) &&
		    !Input.GetKey(KeyCode.LeftArrow) && 
		    !Input.GetKey(KeyCode.A) &&
		    !Input.GetKey(KeyCode.D)) 
			acceleration= 0;
			
		handleSprinting();
		handleJumping();
		change_velocity();
		
	}
	
	void change_velocity(){
		if(!isSprinting && !slowingDown){
			if (peo.vel.x < -maxSpeed.x && acceleration < 0) return;
			if (peo.vel.x > maxSpeed.x && acceleration > 0) return;
		}
		else if(slowingDown){
			if (peo.vel.x < -maxSpeed.x) acceleration = accel_speed;
			if (peo.vel.x > maxSpeed.x) acceleration = -accel_speed;
		}
		else{
			if (peo.vel.x < -maxSprintX && acceleration < 0) return;
			if (peo.vel.x > maxSprintX && acceleration > 0) return;
		}
		vel = peo.vel;
		vel.x += acceleration * Time.deltaTime;
		//slow him down due to friction
		if (acceleration == 0){
			vel.x *= groundMomentumX;
			//kill velocity because *= would never actually hit 0
			if (Mathf.Abs(vel.x) < killVelThreshold){
				vel.x = 0;
			}
		}
		if (Mathf.Sign(peo.vel.x) != Mathf.Sign(acceleration)) 
			vel.x += acceleration * Time.deltaTime * slowDownFactor; 
		peo.vel = vel;
		
	}
	
	void handleJumping(){
		vel = peo.vel;
		
		// Jumping with A (which is x or .)
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period)) {
			// Jump if you're grounded
			if (grounded || isJumping) {
				vel.y = jumpVel;
				// Jumping will set ground = null
				peo.ground = null; 
				isJumping = true;
				stopHeight = peo.transform.position.y + maxJumpHeight;
			}
		}
		//to continue Jumping
		if (isJumping){
			if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Period)){
				if (peo.transform.position.y > stopHeight){
					isJumping = false;
				}
				else{
					vel.y = jumpVel;
				}
			}
			else{
				isJumping = false;
			}
		}
		peo.vel = vel;
	}
	
	void handleSprinting(){
		if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Comma)){
			isSprinting = true;
		}
		else{
			if(!isSprinting && !slowingDown) return;
			isSprinting = false;
			if (Mathf.Abs(peo.vel.x) > maxSpeed.x ){
				slowingDown = true;
			}
			if (Mathf.Abs(peo.vel.x) <= maxSpeed.x ){
				slowingDown = false;
			}
		}
	}
	
}
