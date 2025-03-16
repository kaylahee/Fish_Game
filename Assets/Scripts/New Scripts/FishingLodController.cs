using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLodController : MonoBehaviour
{
    // 내려갈 수 있는 제한선
    [SerializeField]
    private float limitY;

    private float downSpeed = 0.5f;
    public float upSpeed = 1.5f;

    private bool isDetecting = false;
    public bool isReturning = false;
    private float DetectTime;

	SpawnManager spawnManager;

	public GameObject caughtFish = null;

    // Start is called before the first frame update
    void Start()
    {
		spawnManager = FindObjectOfType<SpawnManager>();
	}

	// Update is called once per frame
	void Update()
	{
		// 제한선보다 위에 있을 때 아래로 이동
		if (transform.position.y >= limitY)
		{
			transform.position += Vector3.down * downSpeed * Time.deltaTime;
		}

		float threshold = 0.05f; // 5cm 정도의 오차 허용

		if (Mathf.Abs(transform.position.y - limitY) <= threshold)
		{
			isDetecting = true;
		}

		if (isDetecting)
		{
			DetectTime += Time.deltaTime;
			// 탐지 시간이 2초 정도되면
			if (DetectTime >= 2.0f)
			{
				isDetecting = false; // 탐지 종료
				isReturning = true;  // 원래 위치로 이동 시작
				DetectTime = 0f;
			}
		}

		// 원래 위치로 올라오는 코드
		if (isReturning)
		{
			if (transform.position.y <= 9f)
			{
				transform.position += Vector3.up * upSpeed * Time.deltaTime;
			}

			if (Mathf.Abs(transform.position.y - 9f) <= threshold)
			{
				if (caughtFish != null)
				{
					if (caughtFish.name.Contains("Player_1"))
					{
						caughtFish.GetComponent<PlayerController>()._curHp = 0;
						float t = 0f;
						t += Time.deltaTime;

						if (t >= 2f)
						{
							Destroy(gameObject);
						}
					}
					else
					{
						Destroy(gameObject);
					}
				}

				Destroy(gameObject);

				spawnManager.fishingLineCount--;

				isReturning = false;
				upSpeed = 1.5f;
			}
		}

		if (caughtFish != null)
		{
			if (caughtFish.name == "Player_1")
			{
				//caughtFish.GetComponent<PlayerController>().isCaught = true;
			}
			isReturning = true;
		}
	}
}
