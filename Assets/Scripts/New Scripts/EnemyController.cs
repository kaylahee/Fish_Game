using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	/*public int scorePoint = 50;

	public float currentHP;
	private PlayerHP playerHP;

	[SerializeField]
	private StageData stageData;*/

	private Rigidbody2D rg;
	private PlayerController playerController;

	private float moveSpeed = 1.0f;
	private float fishingmMoveSpeed = 0.005f;

	private float cycleTime = 2.0f;

	public int enemyStage;

	private bool isGoingDown = true;
	private bool isGoingUp = false;
	private float timer = 0f;

	private float maxY;
	private float minY;

	// Start is called before the first frame update
	void Start()
	{
		rg = GetComponent<Rigidbody2D>();

		if (gameObject.CompareTag("FishingShort"))
		{
			maxY = transform.position.y;
			minY = 3.5f;
		}

		if (gameObject.CompareTag("FishingLong"))
		{
			maxY = transform.position.y;
			minY = 2.5f;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (gameObject.CompareTag("FishingShort") || gameObject.CompareTag("FishingLong"))
		{
			if (transform.position.y > minY && isGoingDown)
			{
				transform.position += Vector3.down * fishingmMoveSpeed;
			}
			else if (transform.position.y < maxY && !isGoingDown)
			{
				transform.position += Vector3.up * fishingmMoveSpeed;
			}

			// �ֱ⸦ �������� �������� ������ ��ȯ
			if (isGoingDown && transform.position.y <= minY) // ������ ���� minY�� �����ϸ� ���� ��ȯ
			{
				timer += Time.deltaTime;
				if (timer >= cycleTime) // �ֱⰡ �� �Ǹ� ���� ��ȯ
				{
					isGoingDown = false; // �ö󰡵��� ����
					timer = 0f;
				}
			}
			else if (!isGoingDown && transform.position.y >= maxY) // �ö� ���� maxY�� �����ϸ� ���� ��ȯ
			{
				timer += Time.deltaTime;
				if (timer >= cycleTime) // �ֱⰡ �� �Ǹ� ���� ��ȯ
				{
					isGoingDown = true; // ���������� ����
					timer = 0f;
				}
			}

			if (transform.position.y == maxY)
			{
				Destroy(gameObject);
				FindObjectOfType<SpawnManager>().fishingLineCount--;
			}
		}
	}

	private void FixedUpdate()
	{
		if (gameObject.CompareTag("Feed"))
		{
			if (transform.position.x >= 10f)
			{
				rg.velocity = new Vector2(-1, rg.velocity.x);
			}
			else if (transform.position.x <= -10f)
			{
				rg.velocity = new Vector2(1, rg.velocity.x);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		
	}
}