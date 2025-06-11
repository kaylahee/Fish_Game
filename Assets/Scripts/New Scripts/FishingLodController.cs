using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLodController : MonoBehaviour
{
	[SerializeField]
	private float limitY;

	private float downSpeed = 0.5f;
	public float upSpeed = 1.5f;

	// ����� ã�� ���� ��Ÿ���� ����
	private bool isDetecting = false;
	// �ٽ� �ö󰡴� ����
	public bool isReturning = false;

	// ����� ã�� �ð�
	private float DetectTime;
	// Caught Player �� 2�� ������ ���� ����
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

		// ���� �ưų�, ���� ����Ⱑ ������ ��� ��� ����
		if (caughtFish != null || !dayAndnightCycle.isNight)
		{
			isDetecting = false;
			isReturning = true;
		}

		// �������� �������� ����
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

		// ���� ��ġ�� �ö���� ����
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
		// ���� ���� ����Ⱑ �ִٸ�
		if (caughtFish != null)
		{
			// �װ� �÷��̾���
			if (caughtFish.name.Contains("Player_1"))
			{
				Debug.Log("Caught Player");
				// �÷��̾��� ü���� 0
				caughtFish.GetComponent<PlayerController>()._curHp = 0;

				returnDelay += Time.deltaTime;
				if (returnDelay >= 2f)
				{
					spawnManager.fishingLineCount--; // fishingLineCount ����
					Destroy(gameObject);
					isReturning = false;
					upSpeed = 1.5f;
				}
				return; // ���⼭ ���� ���� (�Ʒ� �ڵ� ���� ����)
			}
			// ���� �װ� ���̶��
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

		// fishingLineCount ����
		spawnManager.fishingLineCount--; 
		Destroy(gameObject);
		isReturning = false;
		upSpeed = 1.5f;
	}
}
