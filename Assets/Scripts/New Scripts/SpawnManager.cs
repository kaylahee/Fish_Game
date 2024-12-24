using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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

	private int feed1Count = 0;
	private int feed2Count = 0;
	private int feed3Count = 0;

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(SpawnFeed1());
		StartCoroutine(SpawnFeed2());
		StartCoroutine(SpawnFeed3());
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

			Instantiate(randomFeed, new Vector3(randomX, randomY, 0f), Quaternion.identity);

			feed3Count++;

			// 5초 후에 다시 스폰
			yield return new WaitForSeconds(15f);
		}
	}
}
