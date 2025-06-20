using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		// �θ�(FishingShort/FishingLong)�� ��ũ��Ʈ�� ã�Ƽ� �÷��̾ �����
		FishingLodController fishingLod = transform.parent.GetComponent<FishingLodController>();

		if (fishingLod.caughtFish == null)
		{
			if (other.CompareTag("Player"))
			{
				// ���˴뿡 ����Ⱑ �����ٴ� ���� ��Ÿ��
				fishingLod.caughtFish = other.transform.parent.gameObject;

				// ���� ������ Caught �̸��� �߰���
				other.transform.parent.gameObject.name += "Caught";
				// �÷��̾ �������� �ڽ����� �����Ͽ� ���� �̵��ϵ��� ��
				other.transform.parent.gameObject.transform.SetParent(transform.parent);
			}

			if (other.CompareTag("Feed"))
			{
				fishingLod.caughtFish = other.gameObject;
				other.name += "Caught";

				other.transform.SetParent(transform.parent);
			}
		}
	}

}
