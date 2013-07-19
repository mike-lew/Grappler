using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {
	public Camera cam_;
	public GameObject arrow_;
	public bool scriptEnabled_ = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetMouseButtonDown(0) && scriptEnabled_) {
			ShootArrow();
		}
	}
	
	private void ShootArrow() {
		Ray ray = new Ray(transform.position, cam_.ScreenPointToRay(Input.mousePosition).GetPoint(100) - transform.position);
		//Debug.Log(ray);
		//RaycastHit[] hits;
		//hits = Physics.RaycastAll(ray, 100);
		//foreach (RaycastHit hit in hits) {
		//}
		GameObject go = (GameObject)Instantiate(arrow_); 
		arrow_.transform.position = gameObject.transform.position + gameObject.transform.forward;
		arrow_.transform.forward = ray.direction;
		go.rigidbody.velocity = rigidbody.velocity;
		go.rigidbody.AddForce(ray.direction * 1000 + Vector3.up * 100);
	}
}
