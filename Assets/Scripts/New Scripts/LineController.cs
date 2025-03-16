using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Feed"))
		{
			// ��(FishingLine)�� �浹�� ���
			if (CompareTag("FishingLine"))
			{
				Debug.Log("Player touched the fishing line! Moving up.");

				// �θ�(FishingShort/FishingLong)�� ��ũ��Ʈ�� ã�Ƽ� �������� �ø���
				transform.parent.GetComponent<FishingLodController>().isReturning = true;
			}
		}
	}

}
