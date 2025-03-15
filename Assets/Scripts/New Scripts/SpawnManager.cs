using System.Collections;
using System.Collections.Generic;
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

	private int feed1Count = 0;
	private int feed2Count = 0;
	private int feed3Count = 0;

	public int fishingLineCount = 0;

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
		if (dn.isNight)
		{
			Debug.Log("Night");
			StartCoroutine(SpawnFishingLine());
		}
		else
		{
			StopCoroutine(SpawnFishingLine());
		}
	}

	private IEnumerator SpawnFeed1()
	{
		// feed_1의 스폰은 3초 후 시작하고 5초마다 반복
		yield return new WaitForSeconds(5f);

		while (feed1Count < maxFeed1Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -10f) : Random.Range(10f, 15f);
			float randomY = Random.Range(-3f, 3f);

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

			Instantiate(randomFeed, new Vector3(randomX, randomY, 0f), Quaternion.identity);

			feed1Count++;

			// 5초 후에 다시 스폰
			yield return new WaitForSeconds(5f);
		}
	}

	private IEnumerator SpawnFeed2()
	{
		// feed_1의 스폰은 3초 후 시작하고 10초마다 반복
		yield return new WaitForSeconds(7f);

		while (feed2Count < maxFeed2Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -10f) : Random.Range(10f, 15f);
			float randomY = Random.Range(-3f, 3f);

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

			// 5초 후에 다시 스폰
			yield return new WaitForSeconds(10f);
		}
	}

	private IEnumerator SpawnFeed3()
	{
		// feed_1의 스폰은 3초 후 시작하고 10초마다 반복
		yield return new WaitForSeconds(9f);

		while (feed3Count < maxFeed3Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -10f) : Random.Range(10f, 15f);
			float randomY = Random.Range(-3f, 3f);

			GameObject randomFeed = feed_3[Random.Range(0, feed_3.Length)];

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

			// 5초 후에 다시 스폰
			yield return new WaitForSeconds(15f);
		}
	}

	private IEnumerator SpawnFishingLine()
	{
		while (fishingLineCount < 2)
		{
			Debug.Log("Spawn fishingLine");
			float randomX = Random.Range(-7.5f, 7.5f);
			float Y = 9f;

			// 이미 스폰된 낚싯줄이 있는지 체크
			bool isPositionOccupied = false;

			// 이미 생성된 낚싯줄들의 X 위치를 체크하여 겹치는지 확인
			foreach (Transform child in transform)
			{
				if (Mathf.Abs(child.position.x - randomX) < 2f)  // 1f는 최소 간격, 필요시 조정
				{
					isPositionOccupied = true;
					break;
				}
			}

			// 위치가 겹치지 않으면 스폰
			if (!isPositionOccupied)
			{
				GameObject randomFishingLine = fishingLine[Random.Range(0, fishingLine.Length)];
				Instantiate(randomFishingLine, new Vector3(randomX, Y, 0f), Quaternion.identity);

				fishingLineCount++;
			}

			// 15초 후에 다시 스폰
			yield return new WaitForSeconds(15f);
		}
	}

}