using UnityEngine;
using UnityEngine.UI;
using System;

public class RestBoostTextController : MonoBehaviour {
	private Text restBoostCountText;

	// Use this for initialization
	void Start () {
		restBoostCountText = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	public void UpdateText (int restBoostCount) {
		string text = "BoostCount: " + restBoostCount.ToString();
		restBoostCountText.text = text;		
	}
}
