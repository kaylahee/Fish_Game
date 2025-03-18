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

		// ȭ�� �����ʿ��� �������� �̵��� ��
		if (transform.position.x > 10f)
		{
			rg.velocity = new Vector2(-1, rg.velocity.y);  // �������� �̵�
		}
		// ȭ�� ���ʿ��� ���������� �̵��� ��
		else if (transform.position.x < -10f)
		{
			rg.velocity = new Vector2(1, rg.velocity.y);  // ���������� �̵�
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
			// ȭ�� ������ ������ ���� (10f�� �������� ȭ�� �ۿ� ������ ��)
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

				// ��ü�� ȭ���� ������ �����ǵ��� ó��
				Destroy(gameObject);
				t = 0f;
			}
		}
	}
}