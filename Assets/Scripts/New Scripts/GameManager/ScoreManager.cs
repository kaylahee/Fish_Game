using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance;

	[Header("현재 점수")]
	public int score = 0;

	public GameObject player;
	PlayerController playerController;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
    {
		if (SceneManager.GetActiveScene().name != "IntroScene")
		{
			playerController = player.GetComponent<PlayerController>();
		}
	}

    void Update()
    {
		if (SceneManager.GetActiveScene().name == "IntroScene")
		{
			score = 0;
		}

		if (SceneManager.GetActiveScene().name != "IntroScene")
		{
			// 플레이어가 비어있을 경우 다시 찾기
			if (player == null)
			{
				player = GameObject.FindWithTag("Player");
				if (player != null)
				{
					playerController = player.GetComponent<PlayerController>();
				}
			}
			else
			{
				UpdateScore();
			}
		}
    }

	public void UpdateScore()
	{
		// 먹이 종류별로 점수 계산
		score = playerController.eatFeed1Count * 10
				+ playerController.eatFeed2Count * 20
				+ playerController.eatFeed3Count * 30;
	}
}
