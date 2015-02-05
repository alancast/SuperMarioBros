using UnityEngine;
using System.Collections;

public class DrawGoombaColliderScript : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(transform.position, transform.localScale);
	}
}
