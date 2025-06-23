using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // 유니티 씬 매니저 사용

public class GameSceneManager : MonoBehaviour
{
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void LoadScene(string sceneName, int resultScore)
	{
		PlayerPrefs.SetInt("ResultScore", resultScore);
		SceneManager.LoadScene(sceneName);
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
