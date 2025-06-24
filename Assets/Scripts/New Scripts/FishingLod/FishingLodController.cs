using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLodController : MonoBehaviour
{
	[SerializeField]
	private float limitY;

	private float downSpeed = 0.5f;
	private float upSpeed = 1.5f;
	
	// ������ ȸ�� ����
	[HideInInspector]
	public bool isReturning = false;

	// ����� Ž�� �ð�
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
		// �������� �������� ����
		if (transform.position.y >= limitY)
		{
			transform.position += Vector3.down * downSpeed * Time.deltaTime;
		}

		// ���� �ưų�, ���� ����Ⱑ ������ ��� ��� ����
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

		// �������� �ö󰡴� ����
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
		// ���� ���� ����Ⱑ �ִٸ�
		if (caughtFish != null)
		{	
			// ���� ����� = �÷��̾�
			if (caughtFish.name.Contains("Player"))
			{
				AfterReturnLogic();
				gameSceneManager.LoadScene("EndScene");
				return;
			}
			// ���� ����� = ����
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
