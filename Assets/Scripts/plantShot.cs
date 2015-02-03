using UnityEngine;
using System.Collections;

public class plantShot : MonoBehaviour {

	public float timeBetweenShots = 2f;
	public float shotDistance = 10f;
	float startTime;
	GameObject mario;
	public float yOffsetForShot = 1f;
	Vector3 shotOrigin;
	public GameObject FireBall;
	public float speedConstant = 1f;



	// Use this for initialization
	void Start () {
		startTime = Time.time;
		mario = GameObject.FindGameObjectWithTag ("Player");
		shotOrigin = this.transform.position;
		shotOrigin.y += yOffsetForShot;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time - startTime > timeBetweenShots) {
			startTime = Time.time;

			if(Mathf.Abs(mario.transform.position.x - this.transform.position.x) < shotDistance){
				GameObject curFireBall = (GameObject)Instantiate(FireBall, shotOrigin, Quaternion.identity);
				FireVelController _FireBall = curFireBall.GetComponent<FireVelController>();

				Vector3 fireVel = mario.transform.position;
				fireVel.y += 1.6f;
				fireVel -= shotOrigin;
				//fireVel = Vector3.Normalize(fireVel);
				//fireVel *= speedConstant;



				_FireBall.fireSpeed = fireVel;
			}
		}
	
	}
}
