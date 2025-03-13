using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private GameObject fish;
    private float speed = 5f;

	// 이동가능한 범위
	private Vector2 p_moveLimit = new Vector2(4.0f, 4.0f);
    [SerializeField]
    private GameObject region;

	public RectTransform hp_front;
    private float _maxHp = 10f;
    public float _curHp = 10f;

	public RectTransform score_front;
	private float _maxScore = 9f;
	public float _curScore = 0f;

	private SpriteRenderer cur_player;
	[SerializeField]
	public Sprite player1;
	[SerializeField]
	public Sprite player2;

    private Transform p_transform;

	public int playerstate = 0;
	public int eatFeedCount = 0;
	private bool isUnBeatTime;

	EnemyController enemyController;

	private void Start()
    {
        cur_player = GetComponentInChildren<SpriteRenderer>();

		hp_front.localScale = new Vector3(_curHp / _maxHp, 1.0f, 1.0f);
		score_front.localScale = new Vector3(_curScore / _maxScore, 1.0f, 1.0f);
	}

    // Update is called once per frame
    private void Update()
    {
        Move();
        /*movement2D.MoveTo(new Vector3(x, y, 0));*/
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
			if (position.y > 0.8f) position.y = 0.8f;
		}

		if (playerstate == 1)
		{
			if (position.x < 0.07f) position.x = 0.07f;
			if (position.x > 0.93f) position.x = 0.93f;
			if (position.y < 0.2f) position.y = 0.2f;
			if (position.y > 0.8f) position.y = 0.8f;
		}

		if (playerstate == 2)
		{
			if (position.x < 0.11f) position.x = 0.11f;
			if (position.x > 0.89f) position.x = 0.89f;
			if (position.y < 0.22f) position.y = 0.22f;
			if (position.y > 0.78f) position.y = 0.78f;
		}

		transform.position = Camera.main.ViewportToWorldPoint(position);
    }


	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 먹이와 닿았을 경우
		if (collision.CompareTag("Feed"))
		{
			Debug.Log(_curScore);
			//Destroy(collision.gameObject);

			enemyController = collision.GetComponent<EnemyController>();

			if (playerstate == 0 && enemyController.enemyStage > 0)
			{
				_curHp -= 20f;
				isUnBeatTime = true;
				StartCoroutine("UnBeatTime");
			}
			else if (playerstate == 1 && enemyController.enemyStage > 1)
			{
				_curHp -= 10f;
				isUnBeatTime = true;
				StartCoroutine("UnBeatTime");
			}
			else
			{
				_curScore += 1f;
				eatFeedCount++;
				Destroy(collision.gameObject);
			}

			hp_front.localScale = new Vector3(_curHp / _maxHp, 1.0f, 1.0f);
			score_front.localScale = new Vector3(_curScore / _maxScore, 1.0f, 1.0f);

            if (eatFeedCount == 3)
            {
                cur_player.sprite = player1;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
				playerstate = 1;
			}
            else if (eatFeedCount == 5)
            {
                cur_player.sprite = player2;
                transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
				playerstate = 2;
			}
		}

		// 낚싯대와 닿았을 경우
        if (collision.CompareTag("Fishing"))
        {
            Debug.Log("Fishing");
            _curHp -= 0f;
			hp_front.localScale = new Vector3(_curHp / _maxHp, 1.0f, 1.0f);
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

		isUnBeatTime = false;

		yield return null;
	}
}