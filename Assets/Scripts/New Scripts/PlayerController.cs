using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public GameObject fish;
    private float speed = 3f;

	public RectTransform hp_front;
    public int _maxHp = 1;
    public int _curHp = 1;

	public RectTransform evol_front;
	private float _maxEvol = 20f;
	public float _curEvol = 0f;

	private SpriteRenderer cur_player;

    private Transform p_transform;

	public int playerstate = 0;
	public int eatFeed1Count = 0;
	public int eatFeed2Count = 0;
	public int eatFeed3Count = 0;

	private bool isUnBeatTime;

	SpawnManager spawnManager;
	EnemyController enemyController;
	FishingLodController fishingLodController;
	EvolutionController evolutionController;

	public bool isCaught = false;

	private void Start()
    {
		cur_player = fish.GetComponentInChildren<SpriteRenderer>();
		spawnManager = FindAnyObjectByType<SpawnManager>();

		evol_front.localScale = new Vector3(_curEvol / _maxEvol, 1.0f, 1.0f);
	}

    // Update is called once per frame
    private void Update()
    {
		// ���忡 ���� Sprite Ȱ��ȭ ����
		for (int i = 0; i < 3; i++)
		{
			if (transform.GetChild(i).gameObject.activeSelf)
			{
				fish = transform.GetChild(i).gameObject;
				cur_player = fish.GetComponentInChildren<SpriteRenderer>();
			}
		}

		// �����ٿ� �Ȱɸ� ���¸� �����̵���
		//if (!isCaught)
		//{
		//	Move();
		//}

		if (!gameObject.name.Contains("Caught"))
		{
			Move();
		}
	}

    // ���� Ű�� ���� �̵� ���� ����
    private void Move()
    {
        // ���� �̵�
        float userInputV = Input.GetAxis("Vertical");
        // ���� �̵�
        float userInputH = Input.GetAxis("Horizontal");

		Vector3 direction = new Vector3(userInputH, userInputV, 0);
		if (direction.magnitude > 1)
			direction = direction.normalized;

		Vector3 targetPos = transform.position + direction * speed * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, targetPos, 0.7f);

		if (direction.magnitude > 0.5f)
		{
			// �¿� ���� �Ǵ��ؼ� Flip ó��
			if (direction.x < 0)
			{
				// ������ �ٶ󺸰� (�⺻ ����)
				fish.transform.localScale = new Vector3(1, 1, 1);

				// ȸ�� ���� ���
				float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
				fish.transform.rotation = Quaternion.Lerp(fish.transform.rotation, targetRotation, Time.deltaTime * 5f);
			}
			else if (direction.x > 0)
			{
				// �������� �ٶ󺸰� (Flip X)
				fish.transform.localScale = new Vector3(-1, 1, 1);

				// ȸ�� ���� ���
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
				fish.transform.rotation = Quaternion.Lerp(fish.transform.rotation, targetRotation, Time.deltaTime * 5f);
			}
			else
			{
				// ���� �̵��� �� ��� (x == 0)
				// ���� �ٶ󺸴� ����(Flip)�� ���� ��/�� ���͸� ���Ƿ� ����
				bool isFacingRight = fish.transform.localScale.x < 0; // Flip X �Ǹ� -1�̴ϱ� ������

				// ��¥ x ������ ������ Atan2 ����
				float pseudoX = isFacingRight ? 1f : -1f;

				float angle = isFacingRight
					? Mathf.Atan2(direction.y, pseudoX) * Mathf.Rad2Deg       // �������� ��
					: Mathf.Atan2(-direction.y, -pseudoX) * Mathf.Rad2Deg;    // ������ ��

				Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
				fish.transform.rotation = Quaternion.Lerp(fish.transform.rotation, targetRotation, Time.deltaTime * 5f);
			}
		}

		// ��� ��輱 ������ �����̵���
		Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

		Vector4[] bounds = new Vector4[] {
			new Vector4(0.03f, 0.97f, 0.2f, 0.95f), // playerstate 0
			new Vector4(0.07f, 0.93f, 0.2f, 0.95f), // playerstate 1
			new Vector4(0.11f, 0.89f, 0.22f, 0.95f) // playerstate 2
		};

		Vector4 b = bounds[playerstate];
		position.x = Mathf.Clamp(position.x, b.x, b.y);
		position.y = Mathf.Clamp(position.y, b.z, b.w);

		transform.position = Camera.main.ViewportToWorldPoint(position);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		fishingLodController = FindObjectOfType<FishingLodController>();
		// ���̿� ����� ���
		if (!gameObject.name.Contains("Caught") && collision.CompareTag("Feed") && !collision.gameObject.name.Contains("Caught"))
		{
			enemyController = collision.GetComponent<EnemyController>();

			if (playerstate == 0 && enemyController.enemyStage > 0)
			{
				_curHp -= 1;
				isUnBeatTime = true;
				StartCoroutine(UnBeatTime());
			}
			else if (playerstate == 1 && enemyController.enemyStage > 1)
			{
				_curHp -= 1;
				isUnBeatTime = true;
				StartCoroutine(UnBeatTime());
			}
			else
			{
				Destroy(collision.gameObject);
				if (enemyController.enemyStage == 0)
				{
					if (_curEvol < 10f)
					{
						_curEvol++;
					}
					eatFeed1Count++;
					spawnManager.feed1Count--;
				}
				else if (enemyController.enemyStage == 1)
				{
					if (_curEvol >= 10f)
					{
						_curEvol++;
					}
					eatFeed2Count++;
					spawnManager.feed2Count--;
				}
				else if (enemyController.enemyStage == 2)
				{
					eatFeed3Count++;
					spawnManager.feed3Count--;
				}
			}

			//hp_front.localScale = new Vector3(_curHp / _maxHp, 1.0f, 1.0f);
			// evol_front�� ũ�� ������Ʈ
			float scale = Mathf.Clamp01(_curEvol / _maxEvol); // 0���� 1 ���̷� ������ ���
			evol_front.localScale = new Vector3(scale, 1.0f, 1.0f);
		}

		if (collision.CompareTag("FishingLine"))
		{
			isCaught = true;
		}
	}

	IEnumerator UnBeatTime()
	{
		int countTime = 0;
		while (countTime < 10)
		{
			speed = 1f;
			if (countTime % 2 == 0)
			{
				cur_player.color = new Color32(255, 255, 255, 90);
			}
			else
			{
				cur_player.color = new Color32(255, 255, 255, 180);
			}

			yield return new WaitForSeconds(0.2f);

			countTime++;
		}

		cur_player.color = new Color32(255, 255, 255, 255);
		speed = 3f;
		isUnBeatTime = false;

		yield return null;
	}
}