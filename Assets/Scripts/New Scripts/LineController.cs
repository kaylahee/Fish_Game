using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Feed"))
		{
			// 줄(FishingLine)과 충돌한 경우
			if (CompareTag("FishingLine"))
			{
				Debug.Log("Player touched the fishing line! Moving up.");

				// 부모(FishingShort/FishingLong)의 스크립트를 찾아서 낚싯줄을 올리기
				transform.parent.GetComponent<FishingLodController>().isReturning = true;
			}
		}
	}

}
