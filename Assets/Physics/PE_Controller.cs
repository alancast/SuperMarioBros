using UnityEngine;
using System.Collections;

public enum MarioState{
	Small,
	Big,
	Fly
}

public class PE_Controller : MonoBehaviour {
	public static PE_Controller instance;
	public static bool BeastMode = false;
	public PE_Obj peo;
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
	//bool for allowing a jump when just killed enemy
	public bool		justKilled = false;
	//set when on alpha leval
	public bool 	isAlpha = false;
	//set when about to attack
	public bool 	isAttackReady = false;
	//check if it was powed
	public bool 	powed = false;
	
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
	public Vector2	maxFlightSpeed = new Vector2( 6.5f, 15 );
	public float 	flightThreshold = 15;
	public float 	flightVelocity = 8;
	private float 	endFlight;
	//time since last button press
	private float 	lastFlightPress;
	//length of time between button presses til velocity drops
	public float	flightButtonThreshold = .15f;
	//down velocity when using the tail while jumping
	public float 	downJumpVelocity = -2f;
	//length of time between button presses til velocity drops
	public float 	downJumpButtonThreshold = .2f;
	//time since last button press
	private float 	lastJumpPress;
	
	//to make sure he doesn't hit two blocks at the same time
	public float 	cantHitTil;
	//how long the kill point for an attack stays there
	public float 	attackUntil;
	//reference to his tail for attacks
	private Transform[] tail;
	
	//Audio
	public AudioSource 	source;
	public AudioClip	shroomGrow;
	public AudioClip	oneUp;
	public AudioClip	pipe;
	public AudioClip	jump;
	public AudioClip	flight;
	public AudioClip	coin;
	public AudioClip	kill;
	public AudioClip	shrink;
	public AudioClip	kickShell;
	public AudioClip	tailSlow;
	
		
	void Awake(){
		instance = this;
		tail = GetComponentsInChildren<Transform>();
		Vector3 temp = tail[1].localPosition;
		temp.y = -60;
		tail[1].localPosition = temp;
		source = GetComponent<AudioSource>();
	}
	
	void Start () {
		peo = GetComponent<PE_Obj>();
	}
	
	void Update () {
		grounded = (peo.ground != null);
		
		if (Input.GetKeyDown(KeyCode.G)){
			BeastMode = !BeastMode;
			if (BeastMode){
				CameraMGR.instance.BeastModeText.text = "Beast Mode Enabled";
			}
			else{
				CameraMGR.instance.BeastModeText.text = "Beast Mode Off";
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			Application.LoadLevel("_Scene_Alex_7");
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			Application.LoadLevel("_Scene_Alpha_2");
		}
		
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
		if (!isFlying){
			//also handles flying start
			handleJumping();
		}
		else{
			handleFlying();
		}
		handleAttack();
		change_velocity();
		
	}
	
	void change_velocity(){
		if(!isSprinting && !slowingDown && !isFlying){
			if (peo.vel.x < -maxSpeed.x && acceleration < 0) return;
			if (peo.vel.x > maxSpeed.x && acceleration > 0) return;
		}
		else if (isFlying){
			if (peo.vel.x < -maxFlightSpeed.x && acceleration < 0) return;
			if (peo.vel.x > maxFlightSpeed.x && acceleration > 0) return;
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
			if (state == MarioState.Fly && Mathf.Abs(vel.x) > flightThreshold && grounded){
				handleFlying();
				return;
			}
			// Jump if you're grounded
			if (grounded || isJumping) {
				vel.y = jumpVel;
				// Jumping will set ground = null
				peo.ground = null; 
				isJumping = true;
				stopHeight = peo.transform.position.y + maxJumpHeight;
			}
			if (state == MarioState.Fly && !isJumping && !justKilled){
				lastJumpPress = Time.time;
			}
			if (justKilled){
				vel.y = jumpVel;
				// Jumping will set ground = null
				isJumping = true;
				justKilled = false;
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
		if (!isJumping && peo.ground == null){
			if (Time.time - lastJumpPress < downJumpButtonThreshold){
				vel.y = downJumpVelocity;
				if (vel.x > maxFlightSpeed.x){
					vel.x = maxFlightSpeed.x;
				}
				if (vel.x < -maxFlightSpeed.x){
					vel.x = -maxFlightSpeed.x;
				}
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
	
	void handleFlying(){
		vel = peo.vel;
		//first call from handleJumping
		if (!isFlying){
			isFlying = true;
			isSprinting = false;
			//set time limit
			endFlight = Time.time + 5;
			vel.y = jumpVel;
			// Jumping will set ground = null
			peo.ground = null; 
			isJumping = true;
			stopHeight = peo.transform.position.y + maxJumpHeight;
		}
		else{
			//hasn't yet hit max height from initial jump
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
			else{
				if (Time.time - lastFlightPress < flightButtonThreshold){
					vel.y = flightVelocity;
					if (vel.x > maxFlightSpeed.x){
						vel.x = maxFlightSpeed.x;
					}
					if (vel.x < -maxFlightSpeed.x){
						vel.x = -maxFlightSpeed.x;
					}
				}
				if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period)) {
					lastFlightPress = Time.time;
				}
			}
		}
		if(Time.time > endFlight){
			isFlying = false;
		}
		peo.vel = vel;
	}
	
	void handleAttack(){
		if (attackUntil < Time.time){
			Vector3 temp = tail[1].localPosition;
			temp.y = -60;
			temp.x = 0;
			tail[1].localPosition = temp;
		}
		if (state != MarioState.Fly) return;
		if (!(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Comma)) && !isAttackReady) return;
		Animator anim = GetComponent<Animator>();
		//to eliminate also setting the trigger on the next update
		if (!isAttackReady){
			anim.SetTrigger("fly_hit");
		}
		//will attack next turn
		if (!isAttackReady){
			isAttackReady = true;
			return;
		}
		isAttackReady = false;
		//spawn the killpoint to the right
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("fly_hit")){
			Vector3 temp = tail[1].localPosition;
			temp.y = 0;
			temp.x = .5f;
			tail[1].localPosition = temp;
			attackUntil = Time.time + .3f;
		}
		//spawn the killpoint to the left
		else{
//			anim.GetCurrentAnimatorStateInfo(0).IsName("fly_hit_left");
			Vector3 temp = tail[1].localPosition;
			temp.y = 0;
			temp.x = -.5f;
			tail[1].localPosition = temp;
			attackUntil = Time.time + .3f;
		}
	}
	
}
