using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	public static FollowCam instance;
	public int maxY;
	public bool _________;
	public GameObject poi;
	public float camZ;
	
	void Awake () {
		instance = this;
		camZ = this.transform.position.z;
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
		// Limit the Y to maximum value
		destination.y = Mathf.Min( maxY, destination.y );
		// Retain a destination.z of camZ 
		destination.z = camZ;
		// Set the camera to the destination 
		transform.position = destination;
	}
}
