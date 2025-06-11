using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		// 부모(FishingShort/FishingLong)의 스크립트를 찾아서 플레이어를 붙잡기
		FishingLodController fishingLod = transform.parent.GetComponent<FishingLodController>();

		if (fishingLod.caughtFish == null)
		{
			if (other.CompareTag("Player"))
			{
				// 낚싯대에 물고기가 잡혔다는 것을 나타냄
				fishingLod.caughtFish = other.transform.parent.gameObject;

				// 잡힌 물고기는 Caught 이름을 추가함
				other.transform.parent.gameObject.name += "Caught";
				// 플레이어를 낚싯줄의 자식으로 설정하여 같이 이동하도록 함
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
