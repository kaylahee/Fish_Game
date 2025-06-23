using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Feed"))
		{
			if (CompareTag("FishingLine"))
			{
				// ≥¨ΩÀ¡Ÿ ø√∏Æ±‚
				var lod = transform.parent.GetComponent<FishingLodController>();
				lod.isReturning = true;

				Debug.Log("Player Collider enabled? " + other.GetComponent<Collider2D>().enabled);
				Debug.Log("Player Rigidbody2D simulated? " + other.GetComponent<Rigidbody2D>().simulated);
			}
		}
	}
}
