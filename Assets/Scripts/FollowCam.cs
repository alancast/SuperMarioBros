﻿using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	public static FollowCam instance;
	public float maxY;
	public float minX;
	private float minY;
	public bool _________;
	public GameObject poi;
	public float camZ;
	
	void Awake () {
		instance = this;
		camZ = this.transform.position.z;
		minY = 0f;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 destination;
		// If there is no poi, return to P:[0,0,0] 
		if (poi == null) {
			print ("no poi");
			destination = Vector3.zero; 
		} 
		else 
		{
			// Get the position of the poi
			destination = poi.transform.position;
		}
		destination.x = Mathf.Max( minX, destination.x );
		if (destination.y > transform.position.y + 5
			&& PE_Controller.instance.isFlying){
			destination.y = destination.y - 5;
			if (destination.y > maxY) destination.y = maxY;
			minY = destination.y;	
		}
		else if(destination.y < transform.position.y - 3){
			destination.y = destination.y + 3;
			if (destination.y < 0) destination.y = 0;
			minY = destination.y;
		}
		//if mario is less than 8 and not flying just ease back to 0
//		else if(destination.y > 3){
//			destination.y -= 
//		}
		else{
			destination.y = minY;
		}
		destination.z = camZ;
		transform.position = destination;
	}
}
