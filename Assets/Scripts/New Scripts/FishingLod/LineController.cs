using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		FishingLodController fishingLodController = GetComponentInParent<FishingLodController>();
		
		// �̹� ���� ����Ⱑ �ִٸ� �ƹ��͵� ���� ����
		if (fishingLodController.caughtFish != null)
		{
			return;
		}

		if (other.CompareTag("Player") || other.CompareTag("Feed"))
		{
			// ������ �ø���
			if (!fishingLodController.isReturning)
			{
				fishingLodController.isReturning = true;
			}
		}
	}
}
