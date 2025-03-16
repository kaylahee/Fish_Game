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
			// 부모(FishingShort/FishingLong)의 스크립트를 찾아서 플레이어를 붙잡기
			FishingLodController fishingLod = transform.parent.GetComponent<FishingLodController>();

			// 바늘(Hook)과 충돌한 경우
			if (CompareTag("Hook") && fishingLod.caughtFish == null)
			{
				Debug.Log("Player caught by the hook!");

				fishingLod.caughtFish = other.transform.parent.gameObject;
				other.transform.parent.gameObject.name += "Caught";

				// 플레이어를 낚싯줄의 자식으로 설정 (같이 이동)
				other.transform.parent.gameObject.transform.SetParent(transform.parent);
			}
		}

		if (other.CompareTag("Feed"))
		{
			// 부모(FishingShort/FishingLong)의 스크립트를 찾아서 플레이어를 붙잡기
			FishingLodController fishingLod = transform.parent.GetComponent<FishingLodController>();

			// 바늘(Hook)과 충돌한 경우
			if (CompareTag("Hook") && fishingLod.caughtFish == null)
			{
				fishingLod.caughtFish = other.gameObject;
				other.name += "Caught";

				other.transform.SetParent(transform.parent);
			}
		}
	}

}
