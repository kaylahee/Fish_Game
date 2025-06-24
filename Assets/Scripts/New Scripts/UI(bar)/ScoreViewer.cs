using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreViewer : MonoBehaviour
{
	[Header("점수 나타내는 UI Text")]
	public TextMeshProUGUI scoreText;
	
	[Header("결과 점수 나타내는 UI Text")]
	public TextMeshProUGUI resultScoreText;

	private void Update()
	{
		if (SceneManager.GetActiveScene().name == "Prototype")
		{
			scoreText.text = "SCORE: " + ScoreManager.Instance.score.ToString();
		}

		if (SceneManager.GetActiveScene().name == "EndScene")
		{
			resultScoreText.text = "SCORE: " + ScoreManager.Instance.score.ToString();
		}
	}
}
