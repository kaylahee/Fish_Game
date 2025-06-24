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
		// 랜덤 각도로 z회전 (회전된 것처럼 보이기 위해)
		float randomAngle = Random.Range(0f, 360f);
		transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);

		// 회전된 Z값 기준으로 이동 방향 계산
		float x = Random.Range(-0.5f, 0.5f);
		float y = -1f;
		moveDirection = new Vector2(x, y).normalized;

		// 정지할 위치
		randomY = Random.Range(-3f, -5f);
	}

    // Update is called once per frame
    private void Update()
    {
		// 대각선 이동
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
