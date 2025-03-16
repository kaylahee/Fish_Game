using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	/*[SerializeField]
    private string nextSceneName;*/

	/*[SerializeField]
    private StageData stageData;
    private Movement2D movement2D;*/

	/*private int score;
    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }*/

	/*private Transform _transformY;
    private Transform _transformX;

    private Vector2 _currentPosY;
    private Vector2 _currentPosX;*/

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

		//hp_front.localScale = new Vector3(_curHp / _maxHp, 1.0f, 1.0f);
		evol_front.localScale = new Vector3(_curEvol / _maxEvol, 1.0f, 1.0f);
	}

    // Update is called once per frame
    private void Update()
    {
		for (int i = 0; i < 3; i++)
		{
			if (transform.GetChild(i).gameObject.activeSelf)
			{
				fish = transform.GetChild(i).gameObject;
			}
		}

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

        if (!(userInputV == 0 && userInputH == 0))
        {
            // 이동
            transform.position += direction * speed * Time.deltaTime;
            //transform.Translate(direction * speed * Time.deltaTime);

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

        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

		if (playerstate == 0)
		{
			if (position.x < 0.03f) position.x = 0.03f;
			if (position.x > 0.97f) position.x = 0.97f;
			if (position.y < 0.2f) position.y = 0.2f;
			if (position.y > 0.95f) position.y = 0.95f;
		}

		if (playerstate == 1)
		{
			if (position.x < 0.07f) position.x = 0.07f;
			if (position.x > 0.93f) position.x = 0.93f;
			if (position.y < 0.2f) position.y = 0.2f;
			if (position.y > 0.95f) position.y = 0.95f;
		}

		if (playerstate == 2)
		{
			if (position.x < 0.11f) position.x = 0.11f;
			if (position.x > 0.89f) position.x = 0.89f;
			if (position.y < 0.22f) position.y = 0.22f;
			if (position.y > 0.95f) position.y = 0.95f;
		}

		transform.position = Camera.main.ViewportToWorldPoint(position);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		fishingLodController = FindObjectOfType<FishingLodController>();
		// 먹이와 닿았을 경우
		if (collision.CompareTag("Feed") && !collision.gameObject.name.Contains("Caught"))
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
	}

	/*private void LateUpdate()
    {
        // 플레이어 캐릭터가 화면 범위 바깥으로 나가지 못하도록 함
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }*/

	/*public void OnDie()
    {
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene(nextSceneName);
    }*/

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