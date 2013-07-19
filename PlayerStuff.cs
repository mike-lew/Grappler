using UnityEngine;
using System.Collections;

public class PlayerStuff : MonoBehaviour {
	public Camera cam_;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.forward = new Vector3(cam_.transform.forward.x, transform.forward.y, cam_.transform.forward.z);
	}
}
