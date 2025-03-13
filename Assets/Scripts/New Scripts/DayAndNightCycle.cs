using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DayAndNightCycle : MonoBehaviour
{
    public SpriteRenderer sr;

	public Color day;
	public Color night;

	public float oneDay;
	public float curTime;

	[Range(0.01f, 0.2f)]
	public float transitionTime;

	public bool isSwap = false;
	public bool isNight = false;

	private void Awake()
	{
		float spriteX = sr.sprite.bounds.size.x;
		float spriteY = sr.sprite.bounds.size.y;

		float screenY = Camera.main.orthographicSize * 2;
		float screenX = screenY / Screen.height * Screen.width;
		transform.localScale = new Vector2(Mathf.Ceil(screenX / spriteX), Mathf.Ceil(screenY / spriteY));

		sr.color = day;
	}

	private void Update()
	{
		curTime += Time.deltaTime;

		// ÇÏ·ç°¡ ¹Ýº¹µÇµµ·Ï ÇÑ´Ù.
		if (curTime >= oneDay)
		{
			curTime = 0;
		}

		if (!isSwap)
		{
			// ³·->¹ã / ¹ã->³·
			if (Mathf.FloorToInt(oneDay * 0.4f) == Mathf.FloorToInt(curTime))
			{
				isNight = true;
				isSwap = true;
				StartCoroutine(SwapColor(sr.color, night));
			}
			else if (Mathf.FloorToInt(oneDay * 0.9f) == Mathf.FloorToInt(curTime))
			{
				isNight = false;
				isSwap = true;
				StartCoroutine(SwapColor(sr.color, day));
			}
		}
		
	}

	IEnumerator SwapColor(Color start, Color end)
	{
		float t = 0;
		while (t < 1)
		{
			t += Time.deltaTime * (1 / (transitionTime * oneDay));
			sr.color = Color.Lerp(start, end, t);
			yield return null;
		}
		isSwap = false;
	}
}
