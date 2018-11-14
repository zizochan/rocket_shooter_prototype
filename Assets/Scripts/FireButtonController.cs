using UnityEngine;
using UnityEngine.UI;

public class FireButtonController : MonoBehaviour {
	GameObject rocket;
	RocketController rocketScript;

	void Start() {
		rocket = GameObject.Find ("Rocket");
		rocketScript = rocket.GetComponent<RocketController>();
	}

	public void OnClick(Button button) {
		rocketScript.Fire ();
	}

	public void Vanish() {
		gameObject.SetActive (false);
	}
}
