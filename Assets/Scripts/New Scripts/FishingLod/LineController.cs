using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		FishingLodController fishingLodController = GetComponentInParent<FishingLodController>();
		
		// 이미 잡힌 물고기가 있다면 아무것도 하지 않음
		if (fishingLodController.caughtFish != null)
		{
			return;
		}

		if (other.CompareTag("Player") || other.CompareTag("Feed"))
		{
			// 낚싯줄 올리기
			if (!fishingLodController.isReturning)
			{
				fishingLodController.isReturning = true;
			}
		}
	}
}
