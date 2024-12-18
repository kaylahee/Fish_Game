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

    /*private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
    }*/

    private void Start()
    {
        
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
        float userInputV = Input.GetAxis("Vertical");
        float userInputH = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(userInputH, userInputV, 0);
        transform.Translate(direction * speed * Time.deltaTime);
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