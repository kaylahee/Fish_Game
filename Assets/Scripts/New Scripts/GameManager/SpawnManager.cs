using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[Header("먹이 관련")]
    [SerializeField]
    private GameObject[] feed_1;
	[SerializeField]
	private GameObject[] feed_2;
	[SerializeField]
	private GameObject[] feed_3;

	// 최대 스폰 개수 제한
	private int maxFeed1Count = 10;
	private int maxFeed2Count = 5;
	private int maxFeed3Count = 3;

	private List<GameObject> feed1List = new List<GameObject>();
	private List<GameObject> feed2List = new List<GameObject>();
	private List<GameObject> feed3List = new List<GameObject>();

	public float Feed1SpawnTime = 3f;
	public float Feed2SpawnTime = 10f;
	public float Feed3SpawnTime = 10f;

	[Header("낚싯줄 관련")]
	[SerializeField] 
	private GameObject[] fishingLine;
	private bool isFishingLodSpawning = false;
	public int fishingLineCount = 0;

	[Header("잠수부 관련")]
	[SerializeField]
	private GameObject swimmer;
	public int swimmerCount = 0;
	public float SwimmerSpawnTime = 10f;

	[Header("쓰레기 관련")]
	[SerializeField]
	private GameObject[] trash;
	private bool isTrashSpawning = false;
	public int trashCount = 0;
	public float TrashSpawnTime = 6.5f;

	public GameObject player;
	public GameObject dayandnight;

	EvolutionController evolutionController;
	DayAndNightCycle dayAndNightCycle;

	// 먹이들 스폰
	void Start()
    {
		evolutionController = player.GetComponent<EvolutionController>();
		dayAndNightCycle = dayandnight.GetComponent<DayAndNightCycle>();

		StartCoroutine(SpawnFeed(feed1List, feed_1, 0f, () => Feed1SpawnTime, feed1List.Count, maxFeed1Count));
		StartCoroutine(SpawnFeed(feed2List, feed_2, 4.5f, () => Feed2SpawnTime, feed2List.Count, maxFeed2Count));
	}

	void Update()
	{
		// 낮에만 쓰레기 스폰
		if (trashCount < 3 && !isTrashSpawning && !dayAndNightCycle.isNight)
		{
			StartCoroutine(SpawnTrash());
		}

		// 밤에만 낚싯줄 스폰
		if (fishingLineCount < 2 && !isFishingLodSpawning && dayAndNightCycle.isNight)
		{
			StartCoroutine(SpawnFishingLine());
		}

		int prevPlayerState = evolutionController.playerstate;
		// 플레이어가 2단계일때부터 3단계 먹이 스폰
		if (prevPlayerState != 1 && evolutionController.playerstate == 1)
		{
			StartCoroutine(SpawnFeed(feed3List, feed_3, 3f, () => Feed3SpawnTime, feed3List.Count, maxFeed3Count));
		}

		// 플레이어가 3단계일 경우 잠수부 스폰
		if (prevPlayerState != 2 && evolutionController.playerstate == 2)
		{
			StartCoroutine(SpawnSwimmer());
		}
	}

	// 겹치는 곳 없이 스폰되도록 구현
	private bool IsPositionOccupied(Vector3 position, float radius)
	{
		// 해당 위치에 겹치는 Collider2D가 있는지 확인
		Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
		foreach (Collider2D collider in colliders)
		{
			// Collider가 'Feed' 타입의 물체인 경우, 겹친다고 판단
			if (collider.CompareTag("Feed"))
			{
				return true;  
			}
		}
		return false;  
	}

	// 먹이 스폰
	private IEnumerator SpawnFeed(List<GameObject> feedList, GameObject[] feed, float spawnTime, Func<float> getSpawnTime, int getFeedCount, int maxfeedCount)
	{
		// spawnTime 후에 시작하고 repeatTime만큼 반복
		yield return new WaitForSeconds(spawnTime);

		while (getFeedCount < maxfeedCount)
		{
			float randomX = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-15f, -11f) : UnityEngine.Random.Range(11f, 15f);
			float randomY = UnityEngine.Random.Range(-3f, 3f);

			// 겹치지 않는 위치를 찾을 때까지 반복
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f는 겹침을 확인할 반지름 값
			{
				randomX = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-15f, -11f) : UnityEngine.Random.Range(11f, 15f);
				randomY = UnityEngine.Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			// 랜덤 먹이가 나올 수 있도록 설정
			GameObject randomFeed = feed[UnityEngine.Random.Range(0, feed.Length)];

			if (!dayAndNightCycle.isNight && randomFeed.name == "Monkfish")
			{
				randomFeed = feed[UnityEngine.Random.Range(1, feed.Length)];
			}

			if (randomFeed.name == "Monkfish")
			{
				randomY = UnityEngine.Random.Range(-3f, -1f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			float cur_scale_x = randomFeed.transform.localScale.x;
			float cur_scale_y = randomFeed.transform.localScale.y;
			float cur_scale_z = randomFeed.transform.localScale.z;

			// 스폰 위치에 따른 위치 반전
			if (randomX < 0f && cur_scale_x > 0f)
			{
				randomFeed.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}
			else if (randomX > 0f && cur_scale_x < 0f)
			{
				randomFeed.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}

			GameObject newfeed = Instantiate(randomFeed, spawnPosition, Quaternion.identity);
			feedList.Add(newfeed);

			yield return new WaitForSeconds(getSpawnTime());
		}
	}

	// 낚싯줄 스폰
	private IEnumerator SpawnFishingLine()	
	{
		isFishingLodSpawning = true;
		yield return new WaitForSeconds(3f);

		while (fishingLineCount < 3)
		{
			Debug.Log("Spawn fishingLine");
			float randomX = UnityEngine.Random.Range(-7.5f, 7.5f);
			float Y = 9f;

			// 이미 스폰된 낚싯줄이 있는지 체크
			bool isPositionOccupied = false;

			// 이미 생성된 낚싯줄들의 X 위치를 체크하여 겹치는지 확인
			foreach (Transform child in transform) 
			{
				if (Mathf.Abs(child.position.x - randomX) < 2f) 
				{
					isPositionOccupied = true;
					break;
				}
			}

			// 위치가 겹치지 않으면 스폰
			if (!isPositionOccupied)
			{
				GameObject randomFishingLine = fishingLine[UnityEngine.Random.Range(0, fishingLine.Length)];
				GameObject spawnedFishingLine = Instantiate(randomFishingLine, new Vector3(randomX, Y, 0f), Quaternion.identity);

				fishingLineCount++;
			}

			yield return new WaitForSeconds(2f);
		}

		isFishingLodSpawning = false;
	}

	// 잠수부 스폰
	private IEnumerator SpawnSwimmer()
	{
		while (swimmerCount < 1)
		{
			float randomX = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-15f, -11f) : UnityEngine.Random.Range(11f, 15f);
			float randomY = UnityEngine.Random.Range(-3f, 3f);

			// 겹치지 않는 위치를 찾을 때까지 반복
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f는 겹침을 확인할 반지름 값
			{
				randomX = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-15f, -11f) : UnityEngine.Random.Range(11f, 15f);
				randomY = UnityEngine.Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			float cur_scale_x = swimmer.transform.localScale.x;
			float cur_scale_y = swimmer.transform.localScale.y;
			float cur_scale_z = swimmer.transform.localScale.z;

			// 스폰 위치에 따른 위치 반전
			if (randomX < 0f && cur_scale_x > 0f)
			{
				swimmer.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}
			else if (randomX > 0f && cur_scale_x < 0f)
			{
				swimmer.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}

			Instantiate(swimmer, spawnPosition, Quaternion.identity);

			swimmerCount++;

			yield return new WaitForSeconds(SwimmerSpawnTime);
		}
	}

	// 쓰레기 스폰
	private IEnumerator SpawnTrash()
	{
		isTrashSpawning = true;
		yield return new WaitForSeconds(8.5f);

		float randomX = UnityEngine.Random.Range(-7.5f, 7.5f);
		float Y = 9f;

		// 이미 스폰된 쓰레기가 있는지 체크
		bool isPositionOccupied = false;

		// 이미 생성된 쓰레기들의 X 위치를 체크하여 겹치는지 확인
		foreach (Transform child in transform)
		{
			if (Mathf.Abs(child.position.x - randomX) < 2f)
			{
				isPositionOccupied = true;
				break;
			}
		}

		// 위치가 겹치지 않으면 스폰
		if (!isPositionOccupied)
		{
			// 랜덤 쓰레기가 나올 수 있도록 설정
			GameObject randomTrash = trash[UnityEngine.Random.Range(0, trash.Length)];
			GameObject spawnedTrash = Instantiate(randomTrash, new Vector3(randomX, Y, 0f), Quaternion.identity);
			spawnedTrash.transform.SetParent(transform);
			trashCount++;
		}

		yield return new WaitForSeconds(TrashSpawnTime);
		isTrashSpawning = false;
	}
}