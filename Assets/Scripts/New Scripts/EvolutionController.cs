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

	void Start()
	{
		playerController = GetComponent<PlayerController>();
		spawnManager = FindObjectOfType<SpawnManager>();

		// ó������ SmallFish�� Ȱ��ȭ
		smallFish.SetActive(true);
		mediumFish.SetActive(false);
		largeFish.SetActive(false);

		playerController.fish = smallFish;
	}

	void Update()
	{
		// ���÷� ������ 1000�� �̻��̸� ��ȭ
		if (playerController._curEvol >= 10 && !mediumFish.activeSelf && isSmall)
		{
			EvolveToMediumFish();
			spawnManager.Feed1SpawnTime = 7f;
			spawnManager.Feed2SpawnTime = 5f;
			spawnManager.Feed2SpawnTime = 10f;
			isSmall = false;
			isMedium = true;
		}
		else if (playerController._curEvol >= 20 && !largeFish.activeSelf && isMedium)
		{
			EvolveToLargeFish();
			spawnManager.Feed1SpawnTime = 10f;
			spawnManager.Feed2SpawnTime = 7f;
			spawnManager.Feed2SpawnTime = 5f;
			isMedium = false;
			isLarge = true;
		}
	}

	// MediumFish�� ��ȭ
	void EvolveToMediumFish()
	{
		smallFish.SetActive(false);
		mediumFish.SetActive(true);
		playerController.fish = mediumFish;
		playerController.playerstate = 1;
		playerController._maxHp = 2;
		playerController._curHp = 2;
		MImage.color = new Color32(255, 255, 255, 255);
	}

	// LargeFish�� ��ȭ
	void EvolveToLargeFish()
	{
		mediumFish.SetActive(false);
		largeFish.SetActive(true);
		playerController.fish = largeFish;
		playerController.playerstate = 2;
		playerController._curHp = 3;
		LImage.color = new Color32(255, 255, 255, 255);
	}

	// ���� �߰��ϴ� ���� �Լ�
	public void AddPoints(int points)
	{
		playerPoints += points;
	}
}
