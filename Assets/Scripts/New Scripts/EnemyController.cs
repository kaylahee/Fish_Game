using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	float t = 0f;
	private Rigidbody2D rg;

	public int enemyStage;
	SpawnManager spawnManager;

	// Start is called before the first frame update
	void Start()
	{
		rg = GetComponent<Rigidbody2D>();
		spawnManager = FindObjectOfType<SpawnManager>();

		// 화면 오른쪽에서 왼쪽으로 이동할 때
		if (transform.position.x > 10f)
		{
			rg.velocity = new Vector2(-1, rg.velocity.y);  // 왼쪽으로 이동
		}
		// 화면 왼쪽에서 오른쪽으로 이동할 때
		else if (transform.position.x < -10f)
		{
			rg.velocity = new Vector2(1, rg.velocity.y);  // 오른쪽으로 이동
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (gameObject.name.Contains("Caught"))
		{
			rg.velocity = new Vector2 (0, rg.velocity.y);
		}

		t += Time.deltaTime;

		if (t > 7f)
		{
			// 화면 밖으로 나가면 삭제 (10f를 기준으로 화면 밖에 나갔을 때)
			if (transform.position.x > 10f || transform.position.x < -10f)
			{
				if (enemyStage == 0)
				{
					spawnManager.feed1Count--;
				}
				else if (enemyStage == 1)
				{
					spawnManager.feed2Count--;
				}
				else if (enemyStage == 2)
				{
					spawnManager.feed3Count--;
				}

				// 물체가 화면을 지나면 삭제되도록 처리
				Destroy(gameObject);
				t = 0f;
			}
		}
	}
}