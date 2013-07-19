using UnityEngine;
using System.Collections;

public class ArrowDecayScript : MonoBehaviour {
	private bool isDead_ = false;
	private float deathCounter_ = 0;
	private bool damageDone_ = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isDead_) {
			deathCounter_ += Time.fixedDeltaTime;
			if (deathCounter_ >= 3) Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.tag != "arrow") {
			isDead_ = true;
		}
		
		if (!damageDone_ && c.transform.root.tag == "Player" && rigidbody.velocity.magnitude > 3 && gameObject.layer == 10) {
			c.transform.root.GetComponent<PlayerHealthScript>().TakeDamage();
			damageDone_ = true;
		}
	}
	
}
