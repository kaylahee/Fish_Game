using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[Header("���� ����")]
    [SerializeField]
    private GameObject[] feed_1;
	[SerializeField]
	private GameObject[] feed_2;
	[SerializeField]
	private GameObject[] feed_3;

	// �ִ� ���� ���� ����
	private int maxFeed1Count = 10;
	private int maxFeed2Count = 5;
	private int maxFeed3Count = 3;

	private List<GameObject> feed1List = new List<GameObject>();
	private List<GameObject> feed2List = new List<GameObject>();
	private List<GameObject> feed3List = new List<GameObject>();

	public float Feed1SpawnTime = 3f;
	public float Feed2SpawnTime = 10f;
	public float Feed3SpawnTime = 10f;

	[Header("������ ����")]
	[SerializeField] 
	private GameObject[] fishingLine;
	private bool isFishingLodSpawning = false;
	public int fishingLineCount = 0;

	[Header("����� ����")]
	[SerializeField]
	private GameObject swimmer;
	public int swimmerCount = 0;
	public float SwimmerSpawnTime = 10f;

	[Header("������ ����")]
	[SerializeField]
	private GameObject[] trash;
	private bool isTrashSpawning = false;
	public int trashCount = 0;
	public float TrashSpawnTime = 6.5f;

	public GameObject player;
	public GameObject dayandnight;

	EvolutionController evolutionController;
	DayAndNightCycle dayAndNightCycle;

	// ���̵� ����
	void Start()
    {
		evolutionController = player.GetComponent<EvolutionController>();
		dayAndNightCycle = dayandnight.GetComponent<DayAndNightCycle>();

		StartCoroutine(SpawnFeed(feed1List, feed_1, 0f, () => Feed1SpawnTime, feed1List.Count, maxFeed1Count));
		StartCoroutine(SpawnFeed(feed2List, feed_2, 4.5f, () => Feed2SpawnTime, feed2List.Count, maxFeed2Count));
	}

	void Update()
	{
		// ������ ������ ����
		if (trashCount < 3 && !isTrashSpawning && !dayAndNightCycle.isNight)
		{
			StartCoroutine(SpawnTrash());
		}

		// �㿡�� ������ ����
		if (fishingLineCount < 2 && !isFishingLodSpawning && dayAndNightCycle.isNight)
		{
			StartCoroutine(SpawnFishingLine());
		}

		int prevPlayerState = evolutionController.playerstate;
		// �÷��̾ 2�ܰ��϶����� 3�ܰ� ���� ����
		if (prevPlayerState != 1 && evolutionController.playerstate == 1)
		{
			StartCoroutine(SpawnFeed(feed3List, feed_3, 3f, () => Feed3SpawnTime, feed3List.Count, maxFeed3Count));
		}

		// �÷��̾ 3�ܰ��� ��� ����� ����
		if (prevPlayerState != 2 && evolutionController.playerstate == 2)
		{
			StartCoroutine(SpawnSwimmer());
		}
	}

	// ��ġ�� �� ���� �����ǵ��� ����
	private bool IsPositionOccupied(Vector3 position, float radius)
	{
		// �ش� ��ġ�� ��ġ�� Collider2D�� �ִ��� Ȯ��
		Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
		foreach (Collider2D collider in colliders)
		{
			// Collider�� 'Feed' Ÿ���� ��ü�� ���, ��ģ�ٰ� �Ǵ�
			if (collider.CompareTag("Feed"))
			{
				return true;  
			}
		}
		return false;  
	}

	// ���� ����
	private IEnumerator SpawnFeed(List<GameObject> feedList, GameObject[] feed, float spawnTime, Func<float> getSpawnTime, int getFeedCount, int maxfeedCount)
	{
		// spawnTime �Ŀ� �����ϰ� repeatTime��ŭ �ݺ�
		yield return new WaitForSeconds(spawnTime);

		while (getFeedCount < maxfeedCount)
		{
			float randomX = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-15f, -11f) : UnityEngine.Random.Range(11f, 15f);
			float randomY = UnityEngine.Random.Range(-3f, 3f);

			// ��ġ�� �ʴ� ��ġ�� ã�� ������ �ݺ�
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f�� ��ħ�� Ȯ���� ������ ��
			{
				randomX = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-15f, -11f) : UnityEngine.Random.Range(11f, 15f);
				randomY = UnityEngine.Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			// ���� ���̰� ���� �� �ֵ��� ����
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

			// ���� ��ġ�� ���� ��ġ ����
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

	// ������ ����
	private IEnumerator SpawnFishingLine()	
	{
		isFishingLodSpawning = true;
		yield return new WaitForSeconds(3f);

		while (fishingLineCount < 3)
		{
			Debug.Log("Spawn fishingLine");
			float randomX = UnityEngine.Random.Range(-7.5f, 7.5f);
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
				GameObject randomFishingLine = fishingLine[UnityEngine.Random.Range(0, fishingLine.Length)];
				GameObject spawnedFishingLine = Instantiate(randomFishingLine, new Vector3(randomX, Y, 0f), Quaternion.identity);

				fishingLineCount++;
			}

			yield return new WaitForSeconds(2f);
		}

		isFishingLodSpawning = false;
	}

	// ����� ����
	private IEnumerator SpawnSwimmer()
	{
		while (swimmerCount < 1)
		{
			float randomX = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-15f, -11f) : UnityEngine.Random.Range(11f, 15f);
			float randomY = UnityEngine.Random.Range(-3f, 3f);

			// ��ġ�� �ʴ� ��ġ�� ã�� ������ �ݺ�
			Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
			while (IsPositionOccupied(spawnPosition, 1f))  // 1f�� ��ħ�� Ȯ���� ������ ��
			{
				randomX = UnityEngine.Random.Range(0f, 1f) > 0.5f ? UnityEngine.Random.Range(-15f, -11f) : UnityEngine.Random.Range(11f, 15f);
				randomY = UnityEngine.Random.Range(-3f, 3f);
				spawnPosition = new Vector3(randomX, randomY, 0f);
			}

			float cur_scale_x = swimmer.transform.localScale.x;
			float cur_scale_y = swimmer.transform.localScale.y;
			float cur_scale_z = swimmer.transform.localScale.z;

			// ���� ��ġ�� ���� ��ġ ����
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

	// ������ ����
	private IEnumerator SpawnTrash()
	{
		isTrashSpawning = true;
		yield return new WaitForSeconds(8.5f);

		float randomX = UnityEngine.Random.Range(-7.5f, 7.5f);
		float Y = 9f;

		// �̹� ������ �����Ⱑ �ִ��� üũ
		bool isPositionOccupied = false;

		// �̹� ������ ��������� X ��ġ�� üũ�Ͽ� ��ġ���� Ȯ��
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
			// ���� �����Ⱑ ���� �� �ֵ��� ����
			GameObject randomTrash = trash[UnityEngine.Random.Range(0, trash.Length)];
			GameObject spawnedTrash = Instantiate(randomTrash, new Vector3(randomX, Y, 0f), Quaternion.identity);
			spawnedTrash.transform.SetParent(transform);
			trashCount++;
		}

		yield return new WaitForSeconds(TrashSpawnTime);
		isTrashSpawning = false;
	}
}