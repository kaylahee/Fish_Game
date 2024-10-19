using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName;

    [SerializeField]
    private StageData stageData;
    private Movement2D movement2D;

    public float moveSpeed = 5.0f;
    public Vector3 moveDirection = Vector3.zero;

    public int score;

    private SpriteRenderer currentImage;
    public Sprite Level1Image;
    public Sprite Level2Image;

    private void Awake()
    {
        currentImage = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // 방향 키를 눌러 이동 방향 설정
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical, 0);
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (score == 100)
        {
            currentImage.sprite = Level1Image;
        }

        if (score == 200)
        {
            currentImage.sprite = Level2Image;
        }
    }

    private void LateUpdate()
    {
        // 플레이어 캐릭터가 화면 범위 바깥으로 나가지 못하도록 함
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }

    public void OnDie()
    {
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene(nextSceneName);
    }
}