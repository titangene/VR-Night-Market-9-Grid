using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public void GoHome() {
		SceneManager.LoadScene("Home");
	}

	public void RestartGame() {
		SceneManager.LoadScene("Game");
	}

	public void PlayGame() {
		SceneManager.LoadScene("Game");
	}
}
