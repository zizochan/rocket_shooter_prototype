using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetButtonController : MonoBehaviour {

	public void OnClick(Button button) {
		SceneManager.LoadScene("MainScene");
	}
}
