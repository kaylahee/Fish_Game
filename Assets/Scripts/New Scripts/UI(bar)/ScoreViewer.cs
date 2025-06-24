using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreViewer : MonoBehaviour
{
	public GameObject player;

	[Header("���� ����")]
	public int score = 0;

	[Header("���� ��Ÿ���� UI Text")]
	public TextMeshProUGUI scoreText;
	
	[HideInInspector]
	public TextMeshProUGUI resultScoreText;       

	PlayerController playerController;

	private void Start()
	{
		playerController = player.GetComponent<PlayerController>();
		UpdateScore();

		// Ȱ��ȭ�� ���� ������� ���
		if (SceneManager.GetActiveScene().name == "EndScene")
		{
			// ��� ���� ǥ��
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
		// ���� �������� ���� ���
		score = playerController.eatFeed1Count * 10
				+ playerController.eatFeed2Count * 20
				+ playerController.eatFeed3Count * 30;

		scoreText.text = "SCORE: " + score.ToString();
	}
}
