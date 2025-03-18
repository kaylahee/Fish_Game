using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] feed_1;
	[SerializeField]
	private GameObject[] feed_2;
	[SerializeField]
	private GameObject[] feed_3;

	[SerializeField] 
	private GameObject[] fishingLine;
	[SerializeField]
	private GameObject background;

	// 최대 스폰 개수 제한
	private int maxFeed1Count = 10;
	private int maxFeed2Count = 5;
	private int maxFeed3Count = 3;

	public int feed1Count = 0;
	public int feed2Count = 0;
	public int feed3Count = 0;

	private bool isSpawning = false;
	public int fishingLineCount = 0;

	public float Feed1SpawnTime = 5f;
	public float Feed2SpawnTime = 10f;
	public float Feed3SpawnTime = 15f;

	public DayAndNightCycle dn;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(SpawnFeed1());
		StartCoroutine(SpawnFeed2());
		StartCoroutine(SpawnFeed3());
	}

	void Update()
	{
		// 밤에만 낚싯줄 스폰
		if (fishingLineCount < 2 && !isSpawning && dn.isNight)
		{
			StartCoroutine(SpawnFishingLine());
		}
	}

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

	private IEnumerator SpawnFeed1()
	{
		// 3초 후 시작하고 5초마다 반복
		yield return new WaitForSeconds(3f);

		while (feed1Count < maxFeed1Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
			float randomY = Random.Range(-3f, 3f);

			// 겹치지 않는 위치를 찾을 때까지 반복
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f는 겹침을 확인할 반지름 값
			{
				randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
				randomY = Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			GameObject randomFeed = feed_1[Random.Range(0, feed_1.Length)];

			float cur_scale_x = randomFeed.transform.localScale.x;
			float cur_scale_y = randomFeed.transform.localScale.y;
			float cur_scale_z = randomFeed.transform.localScale.z;

			// 왼쪽에 스폰됐을 경우 캐릭터가 바라보는 방향이 오른쪽으로
			if (randomX < 0f && cur_scale_x > 0f)
			{
				randomFeed.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}
			else if (randomX > 0f && cur_scale_x < 0f)
			{
				randomFeed.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}

			Instantiate(randomFeed, spawnPosition, Quaternion.identity);

			feed1Count++;

			// 5초 후에 다시 스폰
			yield return new WaitForSeconds(Feed1SpawnTime);
		}
	}

	private IEnumerator SpawnFeed2()
	{
		// 7초 후 시작하고 10초마다 반복
		yield return new WaitForSeconds(7f);

		while (feed2Count < maxFeed2Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
			float randomY = Random.Range(-3f, 3f);

			// 겹치지 않는 위치를 찾을 때까지 반복
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f는 겹침을 확인할 반지름 값
			{
				randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
				randomY = Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			GameObject randomFeed = feed_2[Random.Range(0, feed_2.Length)];

			float cur_scale_x = randomFeed.transform.localScale.x;
			float cur_scale_y = randomFeed.transform.localScale.y;
			float cur_scale_z = randomFeed.transform.localScale.z;

			// 왼쪽에 스폰됐을 경우 캐릭터가 바라보는 방향이 오른쪽으로
			if (randomX < 0f && cur_scale_x > 0f)
			{
				randomFeed.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}
			else if (randomX > 0f && cur_scale_x < 0f)
			{
				randomFeed.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}

			Instantiate(randomFeed, new Vector3(randomX, randomY, 0f), Quaternion.identity);

			feed2Count++;

			yield return new WaitForSeconds(Feed2SpawnTime);
		}
	}

	private IEnumerator SpawnFeed3()
	{
		// 9초 후 시작하고 15초마다 반복
		yield return new WaitForSeconds(9f);

		while (feed3Count < maxFeed3Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
			float randomY = Random.Range(-3f, 3f);

			// 겹치지 않는 위치를 찾을 때까지 반복
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f는 겹침을 확인할 반지름 값
			{
				randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
				randomY = Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			GameObject randomFeed = feed_3[Random.Range(0, feed_3.Length)];
			
			if (randomFeed.name == "MonkFish" && !dn.isNight)
			{
				continue;
			}
			else if (dn.isNight)
			{
				randomFeed = feed_3[0];
			}

			float cur_scale_x = randomFeed.transform.localScale.x;
			float cur_scale_y = randomFeed.transform.localScale.y;
			float cur_scale_z = randomFeed.transform.localScale.z;

			// 왼쪽에 스폰됐을 경우 캐릭터가 바라보는 방향이 오른쪽으로
			if (randomX < 0f && cur_scale_x > 0f)
			{
				randomFeed.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}
			else if (randomX > 0f && cur_scale_x < 0f)
			{
				randomFeed.transform.localScale = new Vector3(cur_scale_x * (-1f), cur_scale_y, cur_scale_z);
			}

			Instantiate(randomFeed, new Vector3(randomX, randomY, 0f), Quaternion.identity);

			feed3Count++;

			yield return new WaitForSeconds(Feed3SpawnTime);
		}
	}

	private IEnumerator SpawnFishingLine()
	{
		isSpawning = true;
		yield return new WaitForSeconds(3f);

		while (fishingLineCount < 3)
		{
			Debug.Log("Spawn fishingLine");
			float randomX = Random.Range(-7.5f, 7.5f);
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
				GameObject randomFishingLine = fishingLine[Random.Range(0, fishingLine.Length)];
				GameObject spawnedFishingLine = Instantiate(randomFishingLine, new Vector3(randomX, Y, 0f), Quaternion.identity);
				spawnedFishingLine.transform.SetParent(transform); 

				fishingLineCount++;
			}

			yield return new WaitForSeconds(2f);
		}
		isSpawning = false;
	}
}