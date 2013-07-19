using UnityEngine;
using System.Collections;

public class BaddieScript : MonoBehaviour {
	public GameObject player_;
	public GameObject arrow_;
	private float minShootInterval_ = 4;
	private float maxShootInterval_ = 8;
	private float shootInterval_;

	// Use this for initialization
	void Start () {
		shootInterval_ = Random.Range(minShootInterval_, maxShootInterval_);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 lookVec = (player_.transform.position - transform.position).normalized;
		lookVec.y = transform.forward.y;
		transform.forward = lookVec;
		
		if (Vector3.Distance(transform.position, player_.transform.position) < 30)
			shootInterval_ -= Time.fixedDeltaTime;
		if (shootInterval_ <= 0) {
			shootInterval_ = Random.Range(minShootInterval_, maxShootInterval_);
			GameObject go = (GameObject)Instantiate(arrow_);
			go.layer = 10;
			foreach (Transform child in go.GetComponentsInChildren<Transform>()) {
				child.gameObject.layer = 10;
			}
			Ray ray = new Ray(transform.position, player_.transform.position - gameObject.transform.position + Vector3.up);
			go.transform.position = gameObject.transform.position + gameObject.transform.forward * 1.2f;
			go.transform.forward = ray.direction;
			
			go.rigidbody.AddForce(ray.direction * 1000 + Vector3.up * 100);
		}
		
		if (transform.position.y < -10) GameObject.Destroy(gameObject);
	}
}
