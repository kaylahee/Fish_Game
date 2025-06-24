using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // ����Ƽ �� �Ŵ��� ���

public class GameSceneManager : MonoBehaviour
{
	public void LoadScene(string sceneName)
	{
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
