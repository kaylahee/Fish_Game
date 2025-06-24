using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private float t = 0f;
	private Rigidbody2D rg;

	public int enemyStage;

	private GameObject gameManager;
	SpawnManager spawnManager;

	private void Start()
	{
		rg = GetComponent<Rigidbody2D>();

		if (gameManager == null)
		{
			gameManager = GameObject.FindWithTag("GameManager");
			if (gameManager != null)
			{
				spawnManager = gameManager.GetComponent<SpawnManager>();
			}
		}

		Move();
	}
	private void Update()
	{
		// 낚싯줄에 닿은 경우 움직이지 않도록 설정
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
				// 물체가 화면을 지나면 삭제되도록 처리
				Destroy(gameObject);
				t = 0f;
			}
		}
	}

	// 먹이 이동
	private void Move()
	{
		// 화면 오른쪽에서 왼쪽으로 이동할 때
		if (transform.position.x > 10f)
		{
			// 왼쪽으로 이동
			rg.velocity = new Vector2(-1, rg.velocity.y);  
		}
		// 화면 왼쪽에서 오른쪽으로 이동할 때
		else if (transform.position.x < -10f)
		{
			// 오른쪽으로 이동
			rg.velocity = new Vector2(1, rg.velocity.y);  
		}
	}
}