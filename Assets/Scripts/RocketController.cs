using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour {
	Rigidbody rigidBody;
	GameObject highScoreText;
	HighScoreTextController highScoreTextScript;
	GameObject RestBoostText;
	RestBoostTextController restBoostTextController;

	GameObject BoostForceInputField;
	GameObject BoostTimeInputField;
	GameObject MassInputField;
	GameObject StabilityInputField;
	GameObject BoostCountInputField;

	private int restBoostCount;
	private float highScore = 0f;
	private int restForceTime; // 残加速回数
	private int gameFlag; // 0:開始前, 1:飛行中, 2:終了

	// パラメーターは全て初期値50, 範囲0~maxにする
	private int boostCount = 1;
	private float boostForce = 50f; // ブースト加速度
	private int boostTime = 50; // 加速有効時間
	private float mass = 50f; // 重さ
	private float stability = 50; // 安定性。加速度がランダムで変わる

	private float drag = 0f; // 空気抵抗

	void Start() {
		gameFlag = 0;

		highScoreText = GameObject.Find ("HighScoreText");
		highScoreTextScript = highScoreText.GetComponent<HighScoreTextController>();
		RestBoostText = GameObject.Find ("RestBoostText");
		restBoostTextController = RestBoostText.GetComponent<RestBoostTextController>();

		BoostForceInputField = GameObject.Find ("BoostForceInputField");
		BoostTimeInputField = GameObject.Find ("BoostTimeInputField");
		MassInputField = GameObject.Find ("MassInputField");
		StabilityInputField = GameObject.Find ("StabilityInputField");
		BoostCountInputField = GameObject.Find ("BoostCountInputField");

		rigidBody = GetComponent<Rigidbody>();

		ResetRocketParams ();
	}

	private void ResetRocketParams() {
		restBoostCount = boostCount;
		restForceTime = 0;
		SetRigidbodyParams ();
	}
		
	// Update is called once per frame
	void Update () {
		if (gameFlag != 1) {
			return;
		}

		AddForce ();
		float nowY = this.transform.position.y;
		if (nowY <= 0) {
			gameFlag = 2;
			return;
		}

		if (highScore < nowY) {
			highScore = nowY;
			highScoreTextScript.UpdateText (highScore);
		}
	}

	private void AddForce() {
		if (restForceTime <= 0) {
			return;
		}

		restForceTime -= 1;

		float rangeMin = this.stability;
		if (rangeMin > 100f) {
			rangeMin = 100f;
		}
		float randomRate = Random.Range(rangeMin, 100f) / 100;

		float boostValue = boostForce * randomRate;
		rigidBody.AddForce(Vector3.up * boostValue);
	}

	private void SetRigidbodyParams() {
		float bodyMass = mass / 10f;
		if (bodyMass < 0.1f) { 
			bodyMass = 0.1f;
		}
		rigidBody.mass = bodyMass;

		float bodyDrag = drag / 25f;
		if (bodyDrag < 0.01f) { 
			bodyDrag = 0.01f;
		}
		rigidBody.drag = bodyDrag;
	}

	public void Fire() {
		if (restBoostCount <= 0) {
			return;
		}

		// 初回はパラメーター設定
		if (gameFlag == 0) {
			SetRocketStatus ();
		}

		gameFlag = 1;

		rigidBody.velocity = Vector3.up;
		restForceTime = boostTime;

		restBoostCount -= 1;
		restBoostTextController.UpdateText (restBoostCount);

		if (restBoostCount <= 0) {
			ValishFireButton ();
		}
	}

	private void SetRocketStatus() {
		string str;

		str = BoostForceInputField.GetComponent<InputField> ().text;
		this.boostForce = ClipToNum (str, 10, 200);

		str = BoostTimeInputField.GetComponent<InputField> ().text;
		this.boostTime = ClipToNum (str, 10, 200);

		str = MassInputField.GetComponent<InputField> ().text;
		this.mass = ClipToNum (str, 10, 200);

		str = StabilityInputField.GetComponent<InputField> ().text;
		this.stability = ClipToNum (str, 10, 100);

		str = BoostCountInputField.GetComponent<InputField> ().text;
		this.boostCount = ClipToNum (str, 1, 5);

		ResetRocketParams ();
	}

	private int ClipToNum(string valueStr, int min, int max) {
		int value;
		int.TryParse(valueStr, out value);

		if (value < min) {
			value = min;
		}
		if (value > max) {
			value = max;
		}

		return value;
	}

	// ランダムで進行方向を狂わせる。空気抵抗で揺れる様を表す
	private void AddRandomForce() {
		int maxStability = 100;
		float tmpStability = (maxStability - this.stability);
		if (tmpStability < 0) {
			return;
		}

		float randX = Random.Range(-tmpStability, tmpStability);
		float randZ = Random.Range(-tmpStability, tmpStability);

		Vector3 randomVector = new Vector3 (randX, 0, randZ);
		rigidBody.AddForce(randomVector);
	}

	void ValishFireButton() {
		GameObject fireButton = GameObject.Find ("FireButton");
		fireButton.GetComponent<FireButtonController>().Vanish();
	}
}
