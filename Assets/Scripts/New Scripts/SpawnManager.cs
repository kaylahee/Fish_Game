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

	// �ִ� ���� ���� ����
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
		// �㿡�� ������ ����
		if (fishingLineCount < 2 && !isSpawning && dn.isNight)
		{
			StartCoroutine(SpawnFishingLine());
		}
	}

	private bool IsPositionOccupied(Vector3 position, float radius)
	{
		// �ش� ��ġ�� ��ġ�� Collider2D�� �ִ��� Ȯ��
		Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
		foreach (Collider2D collider in colliders)
		{
			// Collider�� 'Feed' Ÿ���� ��ü�� ���, ��ģ�ٰ� �Ǵ�
			if (collider.CompareTag("Feed"))
			{
				// ��ħ
				return true;  
			}
		}
		// ��ħ ����
		return false;  
	}

	private IEnumerator SpawnFeed1()
	{
		// 3�� �� �����ϰ� 5�ʸ��� �ݺ�
		yield return new WaitForSeconds(3f);

		while (feed1Count < maxFeed1Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
			float randomY = Random.Range(-3f, 3f);

			// ��ġ�� �ʴ� ��ġ�� ã�� ������ �ݺ�
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f�� ��ħ�� Ȯ���� ������ ��
			{
				randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
				randomY = Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			GameObject randomFeed = feed_1[Random.Range(0, feed_1.Length)];

			float cur_scale_x = randomFeed.transform.localScale.x;
			float cur_scale_y = randomFeed.transform.localScale.y;
			float cur_scale_z = randomFeed.transform.localScale.z;

			// ���ʿ� �������� ��� ĳ���Ͱ� �ٶ󺸴� ������ ����������
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

			// 5�� �Ŀ� �ٽ� ����
			yield return new WaitForSeconds(Feed1SpawnTime);
		}
	}

	private IEnumerator SpawnFeed2()
	{
		// 7�� �� �����ϰ� 10�ʸ��� �ݺ�
		yield return new WaitForSeconds(7f);

		while (feed2Count < maxFeed2Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
			float randomY = Random.Range(-3f, 3f);

			// ��ġ�� �ʴ� ��ġ�� ã�� ������ �ݺ�
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f�� ��ħ�� Ȯ���� ������ ��
			{
				randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
				randomY = Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			GameObject randomFeed = feed_2[Random.Range(0, feed_2.Length)];

			float cur_scale_x = randomFeed.transform.localScale.x;
			float cur_scale_y = randomFeed.transform.localScale.y;
			float cur_scale_z = randomFeed.transform.localScale.z;

			// ���ʿ� �������� ��� ĳ���Ͱ� �ٶ󺸴� ������ ����������
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
		// 9�� �� �����ϰ� 15�ʸ��� �ݺ�
		yield return new WaitForSeconds(9f);

		while (feed3Count < maxFeed3Count)
		{
			float randomX = Random.Range(0f, 1f) > 0.5f ? Random.Range(-15f, -11f) : Random.Range(11f, 15f);
			float randomY = Random.Range(-3f, 3f);

			// ��ġ�� �ʴ� ��ġ�� ã�� ������ �ݺ�
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f�� ��ħ�� Ȯ���� ������ ��
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

			// ���ʿ� �������� ��� ĳ���Ͱ� �ٶ󺸴� ������ ����������
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

			// �̹� ������ �������� �ִ��� üũ
			bool isPositionOccupied = false;

			// �̹� ������ �����ٵ��� X ��ġ�� üũ�Ͽ� ��ġ���� Ȯ��
			foreach (Transform child in transform) 
			{
				if (Mathf.Abs(child.position.x - randomX) < 2f) 
				{
					isPositionOccupied = true;
					break;
				}
			}

			// ��ġ�� ��ġ�� ������ ����
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