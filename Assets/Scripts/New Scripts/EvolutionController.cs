using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionController : MonoBehaviour
{
	public GameObject smallFish;
	public GameObject mediumFish;
	public GameObject largeFish;

	private int playerPoints = 0;

	PlayerController playerController;
	SpawnManager spawnManager;

	private bool isSmall = true;
	private bool isMedium = false;
	private bool isLarge = false;

	public Image MImage;
	public Image LImage;

	private void Start()
	{
		playerController = GetComponent<PlayerController>();
		spawnManager = FindObjectOfType<SpawnManager>();

		// 처음에는 SmallFish만 활성화
		smallFish.SetActive(true);
		mediumFish.SetActive(false);
		largeFish.SetActive(false);

		playerController.fish = smallFish;
	}

	private void Update()
	{
		// 진화 포인트가 10 이상이면 진화
		if (isSmall)
		{
			if (playerController._curEvol >= 10f)
			{
				EvolveToMediumFish();
				ChangeSpawnTime(7f, 5f, 10f);

				isSmall = false;
				isMedium = true;
			}
		}
		else if (isMedium)
		{
			if (playerController._curEvol >= 20f)
			{
				EvolveToLargeFish();
				ChangeSpawnTime(10f, 7f, 5f);

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
		playerController.fish = mediumFish;
		playerController.playerstate = 1;
		playerController._maxHp = 2;
		playerController._curHp = 2;
		MImage.color = new Color32(255, 255, 255, 255);
	}

	// LargeFish로 진화
	private void EvolveToLargeFish()
	{
		mediumFish.SetActive(false);
		largeFish.SetActive(true);
		playerController.fish = largeFish;
		playerController.playerstate = 2;
		playerController._curHp = 3;
		LImage.color = new Color32(255, 255, 255, 255);
	}

	private void ChangeSpawnTime(float feed1_T, float feed2_T, float feed3_T)
	{
		spawnManager.Feed1SpawnTime = feed1_T;
		spawnManager.Feed2SpawnTime = feed2_T;
		spawnManager.Feed2SpawnTime = feed3_T;
	}
}
