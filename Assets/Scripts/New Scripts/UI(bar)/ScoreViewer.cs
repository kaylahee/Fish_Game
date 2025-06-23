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

	InteractionController interactionController;

	private void Start()
	{
		interactionController = player.GetComponentInChildren<InteractionController>();
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
		score = interactionController.eatFeed1Count * 10
				+ interactionController.eatFeed2Count * 20
				+ interactionController.eatFeed3Count * 30;

		scoreText.text = "SCORE: " + score.ToString();
	}
}
