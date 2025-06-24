using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashController : MonoBehaviour
{
	public float moveSpeed = 2f;
	private Vector2 moveDirection;
	private float randomY;

	private bool isMoving = true;

	// Start is called before the first frame update
	private void Start()
    {
		// ���� ������ zȸ�� (ȸ���� ��ó�� ���̱� ����)
		float randomAngle = Random.Range(0f, 360f);
		transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);

		// ȸ���� Z�� �������� �̵� ���� ���
		float x = Random.Range(-0.5f, 0.5f);
		float y = -1f;
		moveDirection = new Vector2(x, y).normalized;

		// ������ ��ġ
		randomY = Random.Range(-3f, -5f);
	}

    // Update is called once per frame
    private void Update()
    {
		// �밢�� �̵�
		if (isMoving)
		{
			transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

			if (transform.position.y <= randomY)
			{
				isMoving = false;
			}
		}
	}
}
