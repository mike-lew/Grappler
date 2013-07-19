using UnityEngine;
using System.Collections;

public class CameraTransparencyScript : MonoBehaviour {
	public GameObject player_;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float distToPlayer = Vector3.Distance(player_.transform.position, transform.position);
		//Debug.Log(gameObject.GetComponent<MouseOrbitZoom>().distance);
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("scenery")) {
			Color matColor = go.renderer.material.color;
			if (Vector3.Distance(go.transform.position, transform.position) < Mathf.Min(10, GetComponent<MouseOrbitZoom>().distance)) {
				matColor.a = .25f;
				go.renderer.material.color = matColor;
			}
			else {
				matColor.a = 1;
				go.renderer.material.color = matColor;
			}
		}
	}
	
	
}
