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
	public float minDist = 3f;
	Animator plant_anim;
	bool alreadyShot = false;


	// Use this for initialization
	void Start () {
		mario = GameObject.FindGameObjectWithTag ("Player");
		shotOrigin = this.transform.position;
		shotOrigin.y += yOffsetForShot;
		plant_anim = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (plant_anim.GetCurrentAnimatorStateInfo (0).IsName ("plant_Top")) {

						if (Mathf.Abs (PE_Controller.instance.transform.position.x - this.transform.position.x) < shotDistance 
								&& Mathf.Abs (PE_Controller.instance.transform.position.y - this.transform.position.y) < (shotDistance - 3)
			    				&& !alreadyShot) {

								GameObject curFireBall = (GameObject)Instantiate (FireBall, shotOrigin, Quaternion.identity);
								FireVelController _FireBall = curFireBall.GetComponent<FireVelController> ();

								Vector3 fireVel = PE_Controller.instance.transform.position;
								fireVel.y += 1.6f;
								fireVel -= shotOrigin;

								if (Mathf.Abs (fireVel.x) < minDist) {

										if (fireVel.x > 0) {
												fireVel.x = minDist;
										} else {
												fireVel.x = -1 * minDist;
										}
								}

								//fireVel = Vector3.Normalize(fireVel);
								//fireVel *= speedConstant;



								_FireBall.fireSpeed = fireVel;
								alreadyShot = true;
						}
				} else {
					alreadyShot = false;
				}
	
	}
}
