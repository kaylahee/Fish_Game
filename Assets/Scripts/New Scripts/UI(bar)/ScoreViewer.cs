using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreViewer : MonoBehaviour
{
	public GameObject player;

	[Header("현재 점수")]
	public int score = 0;

	[Header("점수 나타내는 UI Text")]
	public TextMeshProUGUI scoreText;
	
	[HideInInspector]
	public TextMeshProUGUI resultScoreText;       

	PlayerController playerController;

	private void Start()
	{
		playerController = player.GetComponent<PlayerController>();
		UpdateScore();

		// 활성화된 씬이 엔드씬일 경우
		if (SceneManager.GetActiveScene().name == "EndScene")
		{
			// 결과 점수 표시
			int resultScore = PlayerPrefs.GetInt("ResultScore", 0);
			resultScoreText.text = "FINAL SCORE: " + resultScore.ToString();
		}
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().name == "Prototype")
		{
			UpdateScore();
		}
	}

	public void UpdateScore()
	{
		// 먹이 종류별로 점수 계산
		score = playerController.eatFeed1Count * 10
				+ playerController.eatFeed2Count * 20
				+ playerController.eatFeed3Count * 30;

		scoreText.text = "SCORE: " + score.ToString();
	}
}
