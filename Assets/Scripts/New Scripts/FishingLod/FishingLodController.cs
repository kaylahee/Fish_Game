using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLodController : MonoBehaviour
{
	[SerializeField]
	private float limitY;

	private float downSpeed = 0.5f;
	private float upSpeed = 1.5f;
	
	// 낚싯줄 회수 변수
	[HideInInspector]
	public bool isReturning = false;

	// 물고기 탐색 시간
	private float DetectTime;

	public GameObject caughtFish = null;

	SpawnManager spawnManager;
	DayAndNightCycle dayAndnightCycle;
	GameSceneManager gameSceneManager;

	void Start()
	{
		spawnManager = FindObjectOfType<SpawnManager>();
		dayAndnightCycle = FindObjectOfType<DayAndNightCycle>();
		gameSceneManager = FindObjectOfType<GameSceneManager>();
	}

	void Update()
	{
		// 낚싯줄이 내려가는 로직
		if (transform.position.y >= limitY)
		{
			transform.position += Vector3.down * downSpeed * Time.deltaTime;
		}

		// 낮이 됐거나, 잡힌 물고기가 있으면 즉시 상승 시작
		if (caughtFish != null || !dayAndnightCycle.isNight)
		{
			if (!isReturning)
			{
				isReturning = true;
				upSpeed = 3f;
			}
		}
		else
		{
			if (Mathf.Abs(transform.position.y - limitY) <= 0.05f)
			{
				DetectTime += Time.deltaTime;
				if (DetectTime >= 2.0f)
				{
					if (!isReturning)
					{
						isReturning = true;
						upSpeed = 3f;
					}
					DetectTime = 0f;
				}
			}
		}

		// 낚싯줄이 올라가는 로직
		if (isReturning)
		{
			if (transform.position.y <= 9f)
			{
				transform.position += Vector3.up * upSpeed * Time.deltaTime;
			}

			if (Mathf.Abs(transform.position.y - 9f) <= 0.05f)
			{
				HandleCaughtFish();
			}
		}
	}

	private void HandleCaughtFish()
	{
		// 만약 잡힌 물고기가 있다면
		if (caughtFish != null)
		{	
			// 잡힌 물고기 = 플레이어
			if (caughtFish.name.Contains("Player"))
			{
				AfterReturnLogic();
				gameSceneManager.LoadScene("EndScene");
				return;
			}
			// 잡힌 물고기 = 먹이
			else
			{
				AfterReturnLogic();
			}
		}
		else
		{
			AfterReturnLogic();
		}
	}

	private void AfterReturnLogic()
	{
		spawnManager.fishingLineCount--;
		Destroy(gameObject);
		isReturning = false;
		upSpeed = 1.5f;
	}
}
