using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionController : MonoBehaviour
{
	[Header("현재 물고기")]
	public GameObject fish;
	[HideInInspector]
	public SpriteRenderer cur_player;

	[Header("진화에 따른 물고기 변화")]
	public GameObject smallFish;
	public GameObject mediumFish;
	public GameObject largeFish;

	[Header("물고기 상태 변화")]
	public bool isSmall = true;
	public bool isMedium = false;
	public bool isLarge = false;

	[Header("진화바 관련")]
	public Image evol_front;
	public Image MImage;
	public Image LImage;

	[Header("진화 점수")]
	public float _curEvol = 0f;
	public float _maxEvol = 20f;
	public int playerstate = 0;

	PlayerController playerController;
	SpawnManager spawnManager;

	[Header("게임 메니저")]
	public GameObject gameManager;

	private void Start()
	{
		cur_player = fish.GetComponentInChildren<SpriteRenderer>();

		playerController = GetComponent<PlayerController>();
		spawnManager = gameManager.GetComponent<SpawnManager>();

		smallFish.SetActive(true);
		fish = smallFish;

		evol_front.GetComponent<Image>().fillAmount = 0f;
	}

	private void Update()
	{
		evol_front.fillAmount = _curEvol / _maxEvol;

		// 진화 포인트가 10 이상이면 진화
		if (isSmall)
		{
			if (_curEvol >= 10f)
			{
				EvolveToMediumFish();
				ChangeSpawnTime(5f, 3f, 10f);

				isSmall = false;
				isMedium = true;
			}
		}
		else if (isMedium)
		{
			if (_curEvol >= 20f)
			{
				EvolveToLargeFish();
				ChangeSpawnTime(7f, 5f, 5f);

				isMedium = false;
				isLarge = true;
			}
		}
	}

	// MediumFish로 진화
	private void EvolveToMediumFish()
	{
		smallFish.SetActive(false);
		mediumFish.SetActive(true);
		fish = mediumFish;
		playerstate = 1;
		MImage.color = new Color32(255, 255, 255, 255);
	}

	// LargeFish로 진화
	private void EvolveToLargeFish()
	{
		mediumFish.SetActive(false);
		largeFish.SetActive(true);
		fish = largeFish;
		playerstate = 2;
		LImage.color = new Color32(255, 255, 255, 255);
	}

	// 성장 점수 증가
	public void AddEvolPoint(EnemyController other)
	{
		if (other.enemyStage == 0)
		{
			if (_curEvol < 10f)
			{
				_curEvol++;
			}
		}
		else if (other.enemyStage == 1)
		{
			if (_curEvol >= 10f)
			{
				_curEvol++;
			}
		}
	}

	private void ChangeSpawnTime(float feed1_T, float feed2_T, float feed3_T)
	{
		spawnManager.Feed1SpawnTime = feed1_T;
		spawnManager.Feed2SpawnTime = feed2_T;
		spawnManager.Feed2SpawnTime = feed3_T;
	}
}
