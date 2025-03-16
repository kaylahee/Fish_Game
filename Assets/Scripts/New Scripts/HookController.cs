using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
	PlayerController playerController;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			// �θ�(FishingShort/FishingLong)�� ��ũ��Ʈ�� ã�Ƽ� �÷��̾ �����
			FishingLodController fishingLod = transform.parent.GetComponent<FishingLodController>();

			// �ٴ�(Hook)�� �浹�� ���
			if (CompareTag("Hook") && fishingLod.caughtFish == null)
			{
				Debug.Log("Player caught by the hook!");

				fishingLod.caughtFish = other.transform.parent.gameObject;
				other.transform.parent.gameObject.name += "Caught";

				// �÷��̾ �������� �ڽ����� ���� (���� �̵�)
				other.transform.parent.gameObject.transform.SetParent(transform.parent);
			}
		}

		if (other.CompareTag("Feed"))
		{
			// �θ�(FishingShort/FishingLong)�� ��ũ��Ʈ�� ã�Ƽ� �÷��̾ �����
			FishingLodController fishingLod = transform.parent.GetComponent<FishingLodController>();

			// �ٴ�(Hook)�� �浹�� ���
			if (CompareTag("Hook") && fishingLod.caughtFish == null)
			{
				fishingLod.caughtFish = other.gameObject;
				other.name += "Caught";

				other.transform.SetParent(transform.parent);
			}
		}
	}

}
