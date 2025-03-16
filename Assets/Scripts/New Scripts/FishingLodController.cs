using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLodController : MonoBehaviour
{
    // ������ �� �ִ� ���Ѽ�
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
		// ���Ѽ����� ���� ���� �� �Ʒ��� �̵�
		if (transform.position.y >= limitY)
		{
			transform.position += Vector3.down * downSpeed * Time.deltaTime;
		}

		float threshold = 0.05f; // 5cm ������ ���� ���

		if (Mathf.Abs(transform.position.y - limitY) <= threshold)
		{
			isDetecting = true;
		}

		if (isDetecting)
		{
			DetectTime += Time.deltaTime;
			// Ž�� �ð��� 2�� �����Ǹ�
			if (DetectTime >= 2.0f)
			{
				isDetecting = false; // Ž�� ����
				isReturning = true;  // ���� ��ġ�� �̵� ����
				DetectTime = 0f;
			}
		}

		// ���� ��ġ�� �ö���� �ڵ�
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
