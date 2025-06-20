using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SwimmerController : MonoBehaviour
{
	private Rigidbody2D rg;
	private PlayerController playerController;
	private SpawnManager spawnManager;

	public float followSpeed = 2f;
	public float followDuration = 5f;
	public float moveSpeed = 1.5f;

	private bool isFollowing = false;
	private Vector2 moveDirection;
	private float t = 0f;

	private Vector2 previous;

	private void Start()
	{
		rg = GetComponent<Rigidbody2D>();
		playerController = FindObjectOfType<PlayerController>();
		spawnManager = FindObjectOfType<SpawnManager>();
		StartCoroutine(FollowPlayer());
	}

	private void Update()
	{
		//if (!isFollowing)
		//{
		//	Move();
		//}
	}

	private IEnumerator FollowPlayer()
	{
		isFollowing = true;

		float elapsedTime = 0f;

		while (elapsedTime < followDuration)
		{
			if (playerController != null)
			{
				Vector3 direction = playerController.transform.position - transform.position;
				transform.position += direction.normalized * followSpeed * Time.deltaTime;
			}

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		isFollowing = false;
		previous = rg.velocity;

		Move();
	}

	private void Move()
	{
		float angle = transform.rotation.y;

		// 플레이어가 잠수부보다 오른쪽에 있을 때
		if (playerController.transform.position.x > transform.position.x)
		{
			if (previous.x == -1)
			{
				transform.rotation = Quaternion.Euler(0, angle + 180, 0);
			}
			rg.velocity = new Vector2(-1, rg.velocity.y);  
		}
		// 플레이어가 잠수부보다 왼쪽에 있을 때
		else
		{
			if (previous.x == 1)
			{
				transform.rotation = Quaternion.Euler(0, angle + 180, 0);
			}
			rg.velocity = new Vector2(1, rg.velocity.y);  
		}

		// 시간 누적
		t += Time.deltaTime;

		// 일정 시간이 지나고 화면 밖이면 삭제
		if (t > 7f)
		{
			if (transform.position.x > 12f || transform.position.x < -12f)
			{
				Destroy(gameObject);
			}
		}
	}
}
