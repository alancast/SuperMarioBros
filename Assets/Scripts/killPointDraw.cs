using UnityEngine;
using System.Collections;

public class killPointDraw : MonoBehaviour {
	
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, transform.localScale);
	}
}
