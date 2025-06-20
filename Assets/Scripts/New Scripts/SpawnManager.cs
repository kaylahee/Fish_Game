using System;
using System.Collections;
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

	public int feed1Count = 0;
	public int feed2Count = 0;
	public int feed3Count = 0;

	public float Feed1SpawnTime = 5f;
	public float Feed2SpawnTime = 10f;
	public float Feed3SpawnTime = 15f;

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

	public DayAndNightCycle dn;
	public ScoreViewer sv;
	public PlayerController pc;

	// 먹이들 스폰
	void Start()
    {
		StartCoroutine(SpawnFeed(feed_1, 3f, () => Feed1SpawnTime, () => feed1Count, () => feed1Count++, maxFeed1Count));
		StartCoroutine(SpawnFeed(feed_2, 7f, () => Feed2SpawnTime, () => feed2Count, () => feed2Count++, maxFeed2Count));
		StartCoroutine(SpawnFeed(feed_3, 9f, () => Feed3SpawnTime, () => feed3Count, () => feed3Count++, maxFeed3Count));
	}

	void Update()
	{
		// 낮에만 쓰레기 스폰
		if (trashCount < 3 && !isTrashSpawning && !dn.isNight)
		{
			StartCoroutine(SpawnTrash());
		}

		// 밤에만 낚싯줄 스폰
		if (fishingLineCount < 2 && !isFishingLodSpawning && dn.isNight)
		{
			StartCoroutine(SpawnFishingLine());
		}

		// 플레이어가 3단계일 경우 잠수부 스폰
		if (pc.playerstate == 2)
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
				// 겹침
				return true;  
			}
		}
		// 겹침 없음
		return false;  
	}

	// 먹이 스폰
	private IEnumerator SpawnFeed(GameObject[] feed, float spawnTime, Func<float> getSpawnTime, Func<int> getFeedCount, Action increaseFeedCount, int maxfeedCount)
	{
		// spawnTime 후에 시작하고 repeatTime만큼 반복
		yield return new WaitForSeconds(getSpawnTime());

		while (getFeedCount() < maxfeedCount)
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

			Instantiate(randomFeed, spawnPosition, Quaternion.identity);

			increaseFeedCount();

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