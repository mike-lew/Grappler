using UnityEngine;
using System.Collections;

public class PlayerHealthScript : MonoBehaviour {
	private int hp_ = 6;
	public Texture hp6_;
	public Texture hp5_;
	public Texture hp4_;
	public Texture hp3_;
	public Texture hp2_;
	public Texture hp1_;
	public Texture hp0_;
	private bool isDead_ = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (gameObject.transform.position.y < -10) Death();
	}
	
	void OnGUI() {
		switch(hp_) {
		case 6:
			GUI.DrawTexture(new Rect(120, Screen.height - 120, 100, 100), hp6_);
			break;
		case 5:
			GUI.DrawTexture(new Rect(120, Screen.height - 120, 100, 100), hp5_);
			break;
		case 4:
			GUI.DrawTexture(new Rect(120, Screen.height - 120, 100, 100), hp4_);
			break;
		case 3:
			GUI.DrawTexture(new Rect(120, Screen.height - 120, 100, 100), hp3_);
			break;
		case 2:
			GUI.DrawTexture(new Rect(120, Screen.height - 120, 100, 100), hp2_);
			break;
		case 1:
			GUI.DrawTexture(new Rect(120, Screen.height - 120, 100, 100), hp1_);
			break;
		case 0:
			GUI.DrawTexture(new Rect(120, Screen.height - 120, 100, 100), hp0_);
			break;
		}
		
		if(isDead_) {
			if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 100, 200, 200), "Restart!")) {
				Application.LoadLevel("scene");
			}
		}
	}
	
	public void TakeDamage() {
		hp_ = Mathf.Max(hp_-1, 0);
		if (hp_ == 0 && !isDead_) Death();
	}
	
	private void Death() {
		//rigidbody.isKinematic = false;
		isDead_ = true;
		Destroy(GetComponent<CharacterControls>());
		GetComponent<WeaponSwitchScript>().Death();
	}
}
