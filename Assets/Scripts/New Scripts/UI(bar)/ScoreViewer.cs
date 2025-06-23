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

	InteractionController interactionController;

	private void Start()
	{
		interactionController = player.GetComponentInChildren<InteractionController>();
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
		score = interactionController.eatFeed1Count * 10
				+ interactionController.eatFeed2Count * 20
				+ interactionController.eatFeed3Count * 30;

		scoreText.text = "SCORE: " + score.ToString();
	}
}
