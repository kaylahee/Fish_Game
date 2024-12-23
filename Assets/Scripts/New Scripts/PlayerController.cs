using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	private Transform _transformY;
    private Transform _transformX;

    private Vector2 _currentPosY;
    private Vector2 _currentPosX;

    private float speed = 5f;
    // �̵������� ����
    private Vector2 p_moveLimit = new Vector2(4.0f, 4.0f);
    [SerializeField]
    private GameObject region;

	public RectTransform hp_front;
    private float _maxHp = 10f;
    private float _curHp = 10f;

	public RectTransform score_front;
	private float _maxScore = 9f;
	private float _curScore = 0f;

	private SpriteRenderer cur_player;
	[SerializeField]
	public Sprite player1;
	[SerializeField]
	public Sprite player2;

    private Transform p_transform;

	private void Start()
    {
        cur_player = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        /*movement2D.MoveTo(new Vector3(x, y, 0));*/
    }

    // ���� Ű�� ���� �̵� ���� ����
    private void Move()
    {
        // ���� �̵�
        float userInputV = Input.GetAxis("Vertical");
        // ���� �̵�
        float userInputH = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(-userInputH, userInputV, 0);
        transform.Translate(direction * speed * Time.deltaTime);
        //transform.localPosition = ClampPosition(transform.localPosition);

        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        if (position.x < 0f) position.x = 0f;
        if (position.x > 1f) position.x = 1f;
        if (position.y < 0f) position.y = 0f;
        if (position.y > 1f) position.y = 1f;

        transform.position = Camera.main.ViewportToWorldPoint(position);
    }

    /*private Vector3 ClampPosition(Vector3 pos)
    {
        return new Vector3
        (
            // �¿�� �����̴� �̵�����
            Mathf.Clamp(pos.x, -p_moveLimit.x, p_moveLimit.x),
			Mathf.Clamp(pos.y, -p_moveLimit.y, p_moveLimit.y),
            0
		);
    }*/

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Feed"))
		{
			Debug.Log(_curScore);
			Destroy(collision.gameObject);
            _curScore += 1f;
			hp_front.localScale = new Vector3(_curHp / _maxHp, 1.0f, 1.0f);
			score_front.localScale = new Vector3(_curScore / _maxScore, 1.0f, 1.0f);

            if (_curScore == 3)
            {
                cur_player.sprite = player1;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			}
            else if (_curScore == 5)
            {
                cur_player.sprite = player2;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
		}

        if (collision.CompareTag("Fishing"))
        {
            Debug.Log("Fishing");
            _curHp -= 1f;
			hp_front.localScale = new Vector3(_curHp / _maxHp, 1.0f, 1.0f);
        }
	}

	/*private void LateUpdate()
    {
        // �÷��̾� ĳ���Ͱ� ȭ�� ���� �ٱ����� ������ ���ϵ��� ��
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }*/

	/*public void OnDie()
    {
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene(nextSceneName);
    }*/
}