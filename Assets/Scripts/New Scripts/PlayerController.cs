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
		// 성장에 따른 Sprite 활성화 상태
		for (int i = 0; i < 3; i++)
		{
			if (transform.GetChild(i).gameObject.activeSelf)
			{
				fish = transform.GetChild(i).gameObject;
				cur_player = fish.GetComponentInChildren<SpriteRenderer>();
			}
		}

		// 낚싯줄에 안걸린 상태면 움직이도록
		//if (!isCaught)
		//{
		//	Move();
		//}

		if (!gameObject.name.Contains("Caught"))
		{
			Move();
		}
	}

    // 방향 키를 눌러 이동 방향 설정
    private void Move()
    {
        // 수직 이동
        float userInputV = Input.GetAxis("Vertical");
        // 수평 이동
        float userInputH = Input.GetAxis("Horizontal");

		Vector3 direction = new Vector3(userInputH, userInputV, 0);
		if (direction.magnitude > 1)
			direction = direction.normalized;

		Vector3 targetPos = transform.position + direction * speed * Time.deltaTime;
		transform.position = Vector3.Lerp(transform.position, targetPos, 0.7f);

		if (direction.magnitude > 0.5f)
		{
			// 좌우 방향 판단해서 Flip 처리
			if (direction.x < 0)
			{
				// 왼쪽을 바라보게 (기본 방향)
				fish.transform.localScale = new Vector3(1, 1, 1);

				// 회전 각도 계산
				float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
				fish.transform.rotation = Quaternion.Lerp(fish.transform.rotation, targetRotation, Time.deltaTime * 5f);
			}
			else if (direction.x > 0)
			{
				// 오른쪽을 바라보게 (Flip X)
				fish.transform.localScale = new Vector3(-1, 1, 1);

				// 회전 각도 계산
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
				fish.transform.rotation = Quaternion.Lerp(fish.transform.rotation, targetRotation, Time.deltaTime * 5f);
			}
			else
			{
				// 수직 이동만 할 경우 (x == 0)
				// 현재 바라보는 방향(Flip)에 따라 좌/우 벡터를 임의로 보정
				bool isFacingRight = fish.transform.localScale.x < 0; // Flip X 되면 -1이니까 오른쪽

				// 가짜 x 방향을 설정해 Atan2 계산용
				float pseudoX = isFacingRight ? 1f : -1f;

				float angle = isFacingRight
					? Mathf.Atan2(direction.y, pseudoX) * Mathf.Rad2Deg       // 오른쪽일 때
					: Mathf.Atan2(-direction.y, -pseudoX) * Mathf.Rad2Deg;    // 왼쪽일 때

				Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
				fish.transform.rotation = Quaternion.Lerp(fish.transform.rotation, targetRotation, Time.deltaTime * 5f);
			}
		}

		// 배경 경계선 안으로 움직이도록
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
		// 먹이와 닿았을 경우
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
			// evol_front의 크기 업데이트
			float scale = Mathf.Clamp01(_curEvol / _maxEvol); // 0에서 1 사이로 비율을 계산
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