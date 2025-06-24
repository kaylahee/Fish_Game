using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreViewer : MonoBehaviour
{
	[Header("���� ��Ÿ���� UI Text")]
	public TextMeshProUGUI scoreText;
	
	[Header("��� ���� ��Ÿ���� UI Text")]
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
