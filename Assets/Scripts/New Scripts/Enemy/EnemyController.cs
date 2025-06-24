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
		// �����ٿ� ���� ��� �������� �ʵ��� ����
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
				// ��ü�� ȭ���� ������ �����ǵ��� ó��
				Destroy(gameObject);
				t = 0f;
			}
		}
	}

	// ���� �̵�
	private void Move()
	{
		// ȭ�� �����ʿ��� �������� �̵��� ��
		if (transform.position.x > 10f)
		{
			// �������� �̵�
			rg.velocity = new Vector2(-1, rg.velocity.y);  
		}
		// ȭ�� ���ʿ��� ���������� �̵��� ��
		else if (transform.position.x < -10f)
		{
			// ���������� �̵�
			rg.velocity = new Vector2(1, rg.velocity.y);  
		}
	}
}