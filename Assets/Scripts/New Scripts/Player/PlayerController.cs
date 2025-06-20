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

		// 잡힌 물고기가 아닌 경우
		if (!gameObject.name.Contains("Caught"))
		{
			MoveInArea();
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

		if (!(userInputV == 0 && userInputH == 0))
		{
			// 이동
			transform.position += direction * speed * Time.deltaTime;

			if (userInputH < 0f)
			{
				fish.transform.rotation = Quaternion.Euler(0, 0, 0);

				// 3사분면
				if (userInputV < 0f)
				{
					fish.transform.rotation = Quaternion.Euler(0, 0, 45);
				}
				// 2사분면
				else if (userInputV > 0f)
				{
					fish.transform.rotation = Quaternion.Euler(0, 0, -45);
				}
			}
			else if (userInputH > 0f)
			{
				fish.transform.rotation = Quaternion.Euler(0, 180, 0);

				// 4사분면
				if (userInputV < 0f)
				{
					fish.transform.rotation = Quaternion.Euler(0, 180, 45);
				}
				// 1사분면
				else if (userInputV > 0f)
				{
					fish.transform.rotation = Quaternion.Euler(0, 180, -45);
				}
			}
			else if (userInputV < 0f)
			{
				fish.transform.rotation = Quaternion.Euler(0, 0, 90);
			}
			else if (userInputV > 0f)
			{
				fish.transform.rotation = Quaternion.Euler(0, 0, -90);
			}
		}		
    }

	// 움직일 수 있는 범위 내에서 움직이도록 설정
	private void MoveInArea()
	{
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

	// 먹이 혹은 낚싯줄과 닿았을 떄의 상호작용
	private void OnTriggerEnter2D(Collider2D collision)
	{
		fishingLodController = FindObjectOfType<FishingLodController>();

		// 먹이와 닿았을 경우
		if (!isCaught && collision.CompareTag("Feed") && !collision.gameObject.name.Contains("Caught"))
		{
			enemyController = collision.GetComponent<EnemyController>();

			if (playerstate < enemyController.enemyStage)
			{
				_curHp -= 1;
				isUnBeatTime = true;
				StartCoroutine(UnBeatTime());
			}
			else
			{
				Destroy(collision.gameObject);

				// 진화 포인트
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

			// evol_front의 크기 업데이트
			float scale = Mathf.Clamp01(_curEvol / _maxEvol); // 0에서 1 사이로 비율을 계산
			evol_front.localScale = new Vector3(scale, 1.0f, 1.0f);
		}

		// 낚싯줄과 닿았을 경우
		if (collision.CompareTag("FishingLine"))
		{
			isCaught = true;
		}

		// 쓰레기와 닿았을 경우
		if (collision.CompareTag("Trash"))
		{
			_curHp--;
			isUnBeatTime = true;
			StartCoroutine(UnBeatTime());
		}

		// 잠수부와 닿았을 경우
		if (collision.CompareTag("Swimmer"))
		{
			_curHp = 0;
		}
	}

	// 먹이와 닿았을 경우 무적 시간
	private IEnumerator UnBeatTime()
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