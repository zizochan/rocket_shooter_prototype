using UnityEngine;
using UnityEngine.UI;
using System;

public class HighScoreTextController : MonoBehaviour {
	private Text highScoreText;

	// Use this for initialization
	void Start () {
		highScoreText = this.GetComponent<Text>();
		UpdateText (0f);
	}
	
	public void UpdateText (float score) {
		double trancatedScore = Math.Truncate (score * 100) / 100;
		string text = "Score: " + trancatedScore.ToString();
		highScoreText.text = text;
	}
}
