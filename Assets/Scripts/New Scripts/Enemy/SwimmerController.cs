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

	private IEnumerator FollowPlayer()
	{
		float elapsedTime = 0f;

		Vector3 tempDirection = transform.position;

		while (elapsedTime < followDuration)
		{
			if (playerController != null)
			{
				Vector3 direction = playerController.transform.position - transform.position;
				transform.position += direction.normalized * followSpeed * Time.deltaTime;
				tempDirection = direction.normalized * followSpeed;
			}

			elapsedTime += Time.deltaTime;
		}

		Move(tempDirection);
		yield return null;
	}

	private void Move(Vector2 tempDirection)
	{
		rg.velocity = tempDirection;

		// �÷��̾ ������ Ȥ�� ���ʿ� ��ġ�� ��
		if (playerController.transform.position.x != transform.position.x)
		{
			// ����δ� �������� ���ư��� ��
			float angle = transform.rotation.eulerAngles.y;
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
