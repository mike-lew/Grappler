using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrappleScript : MonoBehaviour {
	public Camera cam_;
	public bool scriptEnabled_ = false;
	
	private bool canShoot_ = true;
	private bool isShooting_ = false;
	private bool isReeling_ = false;
	public bool isAttached_ = false;
	private float linkGenTimer_ = 0;
	private bool attachedToEnemy_ = false;
	
	private GameObject linkTip_ = null;
	private LinkedList<GameObject> linkChain_;
	private Vector3 linkDir_;
	private int linkNum_ = 0;
	private ConfigurableJoint enemyJoint_ = null;
	// Use this for initialization
	void Start () {
		linkChain_ = new LinkedList<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (linkChain_.Count == 0) GetComponent<WeaponSwitchScript>().canSwitch_ = true;
		RaycastHit[] hits;
		Ray ray = cam_.ScreenPointToRay(Input.mousePosition);
		hits = Physics.RaycastAll(ray, 100.0F);
		if (hits != null && hits.Length > 0 && !isShooting_) {
			//Debug.Log(hits[0].point);
			//Debug.DrawRay(transform.position, hits[0].point - transform.position, Color.red);
			if (linkChain_.Count == 0) canShoot_ = true;
		}
		else {
			canShoot_ = false;
		}
		if (canShoot_ && Input.GetMouseButtonDown(0) && scriptEnabled_) {
			Vector3 target;
			bool targetAcquired = false;
			foreach (RaycastHit hit in hits) {
				//check if hitting a transparent or floor tile
				if (hit.collider.renderer.material.color.a < .9f) continue;
				if (Vector3.Distance(transform.position, cam_.transform.position) > 
					Vector3.Distance(hit.collider.transform.position, cam_.transform.position)) {
					continue;
				}
				target = hit.point;
				targetAcquired = true;
				break;
			}
			if (targetAcquired) {
				isShooting_ = true;
				linkTip_ = gameObject;
				linkDir_ = (target - transform.position).normalized;
				linkChain_ = new LinkedList<GameObject>();
				isAttached_ = false;
				canShoot_ = false;
				GetComponent<WeaponSwitchScript>().canSwitch_ = false;
			}
		}
		if (Input.GetMouseButtonDown(1)) {
			isAttached_ = false;
			attachedToEnemy_ = false;
			gameObject.rigidbody.mass = 1;
			if (enemyJoint_ != null) {
				enemyJoint_.xMotion = ConfigurableJointMotion.Free;
				enemyJoint_.yMotion = ConfigurableJointMotion.Free;
				enemyJoint_.zMotion = ConfigurableJointMotion.Free;
				enemyJoint_ = null;
			}
		}
		if (isShooting_) {
			linkGenTimer_ += Time.fixedDeltaTime;
			if (linkGenTimer_ >= .03f) {
				GenerateShootLink();
				linkGenTimer_ = 0;
			}
		}
		else if (!isShooting_ && isAttached_ && linkChain_ != null && linkChain_.Count > 1) {
			FlyReel();
		}
		else if (!isShooting_ && !isAttached_ && linkChain_ != null && linkChain_.Count > 0) {
			linkGenTimer_ += Time.fixedDeltaTime;
			if (linkGenTimer_ >= .05f) {
				linkGenTimer_ = 0;
				Reel();
			}
		}
	}
	
	private void FlyReel() {
//		GameObject link = linkChain_.First.Value;
//		linkChain_.RemoveFirst();
//		GameObject.Destroy(link);
//		link = linkChain_.First.Value;
//		link.GetComponent<ConfigurableJoint>().connectedBody = gameObject.rigidbody;
//		gameObject.rigidbody.velocity = (linkChain_.Last.Value.transform.position - transform.position).normalized * 6f;
		//gameObject.rigidbody.velocity += (linkChain_.Last.Value.transform.position - transform.position).normalized * 10;
		//gameObject.rigidbody.
		if (!attachedToEnemy_)
			gameObject.rigidbody.AddForce((linkChain_.First.Value.transform.position 
				- transform.position).normalized * rigidbody.mass * 9.8f * 10);
	}
	
	private void Reel() {
		GameObject link = linkChain_.Last.Value;
		linkChain_.RemoveLast();
		GameObject.Destroy(link);
	}
	
	private void GenerateShootLink() {
		GameObject link = GameObject.CreatePrimitive(PrimitiveType.Cube);
		link.transform.position = linkTip_.transform.position + linkDir_.normalized / 2;
		link.transform.forward = linkDir_;
		link.transform.localScale = new Vector3(.05f, .05f, .5f);
		link.tag = "link";
		link.layer = 8;
		link.AddComponent<Rigidbody>();
		link.rigidbody.isKinematic = true;
		link.rigidbody.angularDrag = 1;
		link.rigidbody.mass = 1;
		link.AddComponent<ConfigurableJoint>();
		ConfigurableJoint joint = link.GetComponent<ConfigurableJoint>();
		joint.xMotion = ConfigurableJointMotion.Locked;
		joint.yMotion = ConfigurableJointMotion.Locked;
		joint.zMotion = ConfigurableJointMotion.Locked;
		joint.anchor = Vector3.zero;
		joint.connectedBody = linkTip_.rigidbody;
		linkChain_.AddLast(link);
		linkTip_ = link;
		linkNum_++;
		//get attached?
		RaycastHit hit;
		if (link.rigidbody.SweepTest(link.transform.forward, out hit, 0.5f)) {
			isAttached_ = true;
			if (hit.collider.transform.root.tag == "baddie") {
				attachedToEnemy_ = true;
				gameObject.rigidbody.mass = 50;
			}
			Debug.Log(hit.collider.transform.root.tag + " : " + hit.collider.transform.root.name);
		}
		if (isAttached_ || linkNum_ == 30) {
			isShooting_ = false;
			linkTip_ = gameObject;
			linkNum_ = 0;
			foreach (GameObject go in linkChain_) {
				go.rigidbody.isKinematic = false;
			}
			if (isAttached_ && !attachedToEnemy_) linkChain_.Last.Value.rigidbody.isKinematic = true;
			if (attachedToEnemy_) {
				enemyJoint_ = hit.collider.transform.root.GetComponent<ConfigurableJoint>();
				enemyJoint_.connectedBody = linkChain_.Last.Value.rigidbody;
				enemyJoint_.xMotion = ConfigurableJointMotion.Locked;
				enemyJoint_.yMotion = ConfigurableJointMotion.Locked;
				enemyJoint_.zMotion = ConfigurableJointMotion.Locked;
			}
			isReeling_ = true;
		}
	}
	
	public void linkReelCollision() {
		GameObject link = linkChain_.First.Value;
		linkChain_.RemoveFirst();
		GameObject.Destroy(link);
		link = linkChain_.First.Value;
		link.GetComponent<ConfigurableJoint>().connectedBody = gameObject.rigidbody;
		//gameObject.rigidbody.AddForce((link.transform.position - transform.position).normalized * rigidbody.mass * 9.8f);
	}
}
