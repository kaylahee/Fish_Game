using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLodController : MonoBehaviour
{
	[SerializeField]
	private float limitY;

	private float downSpeed = 0.5f;
	public float upSpeed = 1.5f;

	// 물고기 찾는 중을 나타내는 변수
	private bool isDetecting = false;
	// 다시 올라가는 변수
	public bool isReturning = false;

	// 물고기 찾는 시간
	private float DetectTime;
	// Caught Player 후 2초 지연을 위한 변수
	private float returnDelay = 0f; 

	SpawnManager spawnManager;
	public GameObject caughtFish = null;

	DayAndNightCycle dayAndnightCycle;

	void Start()
	{
		spawnManager = FindObjectOfType<SpawnManager>();
		dayAndnightCycle = FindObjectOfType<DayAndNightCycle>();
	}

	void Update()
	{
		float threshold = 0.05f;

		// 낮이 됐거나, 잡힌 물고기가 있으면 즉시 상승 시작
		if (caughtFish != null || !dayAndnightCycle.isNight)
		{
			isDetecting = false;
			isReturning = true;
		}

		// 낚싯줄이 내려가는 로직
		if (transform.position.y >= limitY)
		{
			transform.position += Vector3.down * downSpeed * Time.deltaTime;
		}

		if (Mathf.Abs(transform.position.y - limitY) <= threshold)
		{
			isDetecting = true;
		}

		if (isDetecting)
		{
			DetectTime += Time.deltaTime;
			if (DetectTime >= 2.0f)
			{
				isDetecting = false;
				isReturning = true;
				DetectTime = 0f;
			}
		}

		// 원래 위치로 올라오는 로직
		if (isReturning)
		{
			if (transform.position.y <= 9f)
			{
				transform.position += Vector3.up * upSpeed * Time.deltaTime;
			}

			if (Mathf.Abs(transform.position.y - 9f) <= threshold)
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
			// 그게 플레이어라면
			if (caughtFish.name.Contains("Player_1"))
			{
				Debug.Log("Caught Player");
				// 플레이어의 체력은 0
				caughtFish.GetComponent<PlayerController>()._curHp = 0;

				returnDelay += Time.deltaTime;
				if (returnDelay >= 2f)
				{
					spawnManager.fishingLineCount--; // fishingLineCount 감소
					Destroy(gameObject);
					isReturning = false;
					upSpeed = 1.5f;
				}
				return; // 여기서 실행 종료 (아래 코드 실행 방지)
			}
			// 만약 그게 먹이라면
			else
			{
				EnemyController enemy = caughtFish.GetComponent<EnemyController>();
				if (enemy != null)
				{
					if (enemy.enemyStage == 0) spawnManager.feed1Count--;
					if (enemy.enemyStage == 1) spawnManager.feed2Count--;
					if (enemy.enemyStage == 2) spawnManager.feed3Count--;
				}
			}
		}

		// fishingLineCount 감소
		spawnManager.fishingLineCount--; 
		Destroy(gameObject);
		isReturning = false;
		upSpeed = 1.5f;
	}
}
