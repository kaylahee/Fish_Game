using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackGroundScroller : MonoBehaviour
{
	// ���� ���� �̾����� ���
	[SerializeField]
	private Transform target;
	// �̾����� �� ��� ������ �Ÿ�
	[SerializeField]
	private float scrollAmount;
	// �̵� �ӵ�
	[SerializeField]
	private float moveSpeed;
	// �̵� ����
	[SerializeField]
	private Vector3 moveDirection;

	void Update()
	{
		// ����� moveDirection �������� moveSpeed�� �ӵ��� �̵�
		transform.position += moveDirection * moveSpeed * Time.deltaTime;

		// ����� ������ ������ ����� ��ġ �缳��
		if (transform.position.x <= -scrollAmount)
		{
			transform.position = target.position - moveDirection * scrollAmount;
		}
	}
}
