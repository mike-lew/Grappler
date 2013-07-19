using UnityEngine;
using System.Collections;

public class ReelTriggerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay (Collider c) {
		if (transform.root.GetComponent<GrappleScript>().isAttached_) {
			if (c.gameObject.tag == "link") {
				transform.root.GetComponent<GrappleScript>().linkReelCollision();
			}
		}
	}
}
