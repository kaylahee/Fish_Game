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
				// 낚싯대에 물고기가 잡혔다는 것을 나타냄
				fishingLod.caughtFish = other.transform.parent.gameObject;

				// 잡힌 물고기는 Caught 이름을 추가함
				if (!other.transform.parent.gameObject.name.Contains("Caught"))
				{
					other.transform.parent.gameObject.name += "Caught";
				}
				// 플레이어를 낚싯줄의 자식으로 설정하여 같이 이동하도록 함
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
