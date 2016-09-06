using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	Vector3 lookAt = new Vector3(0,0,0);
	float d = .01f;
	float step=0;
	float radius = 18; //distance from center
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		step += d;
		Vector3 pos = new Vector3 (Mathf.Cos(step)*radius, 0, Mathf.Sin(step)*radius);
		//move camera and then look at lookAt
		transform.position = pos;
		transform.LookAt(lookAt);
	}
}
