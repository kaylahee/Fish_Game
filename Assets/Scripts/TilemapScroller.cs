/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScroller : MonoBehaviour
{
	[SerializeField]
	private Transform target;
	[SerializeField]
	private float scrollRange = 9.9f;
	[SerializeField]
	private float scrollSpeed = 1.0f;
	[SerializeField]
	public Vector3 moveDirection = Vector3.left;

	void Update()
	{
		transform.position -= moveDirection * scrollSpeed * Time.deltaTime;

		if (transform.position.x <= -scrollRange)
		{
			transform.position = target.position + Vector3.right * scrollRange;
		}
	}
}
*/