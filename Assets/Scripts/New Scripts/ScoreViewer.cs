using UnityEngine;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class ScoreViewer : MonoBehaviour
{
	public TextMeshProUGUI scoreText;       // ���� �� ���� ǥ��
	public TextMeshProUGUI resultScoreText;       // ���� �� ���� ǥ��
	public int score = 0;

	PlayerController playerController;

	// Start is called before the first frame update
	void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
		if (SceneManager.GetActiveScene().name == "Prototype")
		{
			UpdateScoreDisplay();
		}

		if (SceneManager.GetActiveScene().name == "EndScene")
		{
			int resultScore = PlayerPrefs.GetInt("ResultScore", 0);
			resultScoreText.text = "FINAL SCORE: " + resultScore.ToString();
		}

		
	}

	public void UpdateScore()
	{
		// ���� �������� ���� ���
		score = playerController.eatFeed1Count * 10
				+ playerController.eatFeed2Count * 20
				+ playerController.eatFeed3Count * 30;

		UpdateScoreDisplay();
	}

	void UpdateScoreDisplay()
	{
		scoreText.text = "SCORE: " + score.ToString();
	}

	// Update is called once per frame
	void Update()
	{
		if (SceneManager.GetActiveScene().name == "Prototype")
		{
			UpdateScore();
		}
	}
}
