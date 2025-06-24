using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		FishingLodController fishingLod = GetComponentInParent<FishingLodController>();

		if (fishingLod.caughtFish == null)
		{
			if (other.CompareTag("Player"))
			{
				// ���˴뿡 ����Ⱑ �����ٴ� ���� ��Ÿ��
				fishingLod.caughtFish = other.transform.parent.gameObject;

				// ���� ������ Caught �̸��� �߰���
				if (!other.transform.parent.gameObject.name.Contains("Caught"))
				{
					other.transform.parent.gameObject.name += "Caught";
				}
				// �÷��̾ �������� �ڽ����� �����Ͽ� ���� �̵��ϵ��� ��
				other.transform.parent.gameObject.transform.SetParent(transform.parent);
			}

			if (other.CompareTag("Feed"))
			{
				fishingLod.caughtFish = other.gameObject;
				other.gameObject.name += "Caught";
				if (!other.gameObject.name.Contains("Caught"))
				{
					other.gameObject.name += "Caught";
				}

				other.gameObject.transform.SetParent(transform.parent);
			}
			
			gameObject.GetComponent<CircleCollider2D>().enabled = false;
			other.GetComponent<Collider2D>().enabled = false;
		}
	}

}
