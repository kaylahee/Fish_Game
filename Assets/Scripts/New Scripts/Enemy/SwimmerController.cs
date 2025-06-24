using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class SwimmerController : MonoBehaviour
{
	private Rigidbody2D rg;

	public float followSpeed = 2f;
	public float followDuration = 5f;
	public float moveSpeed = 1.5f;

	private Vector2 moveDirection;
	private float t = 0f;

	private Vector2 previous;

	private GameObject player;
	private GameObject gameManager;

	SpawnManager spawnManager;

	private void Start()
	{
		rg = GetComponent<Rigidbody2D>();

		if (player == null)
		{
			player = GameObject.FindWithTag("Player");
		}

		if (gameManager == null)
		{
			gameManager = GameObject.FindWithTag("GameManager");
			if (gameManager != null)
			{
				spawnManager = gameManager.GetComponent<SpawnManager>();
			}
		}

		StartCoroutine(FollowPlayer());
	}

	private IEnumerator FollowPlayer()
	{
		float elapsedTime = 0f;

		Vector3 tempDirection = transform.position;

		while (elapsedTime < followDuration)
		{
			if (player != null)
			{
				Vector2 direction = player.transform.position - transform.position;
				transform.position += (Vector3)(direction.normalized * followSpeed * Time.deltaTime);
				tempDirection = direction.normalized * followSpeed;
			}

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Move(tempDirection);
	}

	private void Move(Vector2 tempDirection)
	{
		rg.velocity = tempDirection;

		// ����δ� �������� ���ư��� ��
		float angle = transform.rotation.eulerAngles.y;

		// �÷��̾ �����ʿ� ��ġ�� ��
		if (player.transform.position.x > transform.position.x)
		{
			transform.rotation = Quaternion.Euler(0, (angle + 180) % 360, 0);
			rg.velocity = new Vector2(-1 * rg.velocity.x, rg.velocity.y);
		}
		else
		{
			rg.velocity = new Vector2(rg.velocity.x, rg.velocity.y);
		}

		// �ð� ����
		t += Time.deltaTime;

		// ���� �ð��� ������ ȭ�� ���̸� ����
		if (t > 7f)
		{
			if (transform.position.x > 12f || transform.position.x < -12f)
			{
				Destroy(gameObject);
			}
		}
	}
}
