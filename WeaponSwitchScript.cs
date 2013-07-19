using UnityEngine;
using System.Collections;

public class WeaponSwitchScript : MonoBehaviour {
	//private GrappleScript wep1_;
	//private ArrowScript wep2_;
	public int curWep_ = 1; //1 = grapple, 2 = arrow
	public bool canSwitch_ = true;
	public Texture wep1_;
	public Texture wep2_;
	public bool isDead_;

	// Use this for initialization
	void Start () {
		//wep1_ = GetComponent<GrappleScript>();
		//wep2_ = GetComponent<ArrowScript>();
		curWep_ = 1;
		GetComponent<GrappleScript>().scriptEnabled_ = true;
		GetComponent<ArrowScript>().scriptEnabled_ = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead_) return;
		if (Input.GetKeyDown(KeyCode.Alpha1) && canSwitch_) {
			curWep_ = 1;
			GetComponent<GrappleScript>().scriptEnabled_ = true;
			GetComponent<ArrowScript>().scriptEnabled_ = false;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2) && canSwitch_) {
			curWep_ = 2;
			GetComponent<GrappleScript>().scriptEnabled_ = false;
			GetComponent<ArrowScript>().scriptEnabled_ = true;
		}
	}
	
	void OnGUI() {
		switch (curWep_) {
		case 1:
			GUI.DrawTexture(new Rect(20, Screen.height - 180, 100, 100), wep1_);
			break;
		case 2:
			GUI.DrawTexture(new Rect(20, Screen.height - 180, 100, 100), wep2_);
			break;
		}
	}
	
	public void Death() {
		GetComponent<GrappleScript>().scriptEnabled_ = false;
		GetComponent<ArrowScript>().scriptEnabled_ = false;
	}
}
