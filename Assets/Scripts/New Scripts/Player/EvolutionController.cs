using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionController : MonoBehaviour
{
	[Header("���� �����")]
	public GameObject fish;
	[HideInInspector]
	public SpriteRenderer cur_player;

	[Header("��ȭ�� ���� ����� ��ȭ")]
	public GameObject smallFish;
	public GameObject mediumFish;
	public GameObject largeFish;

	[Header("����� ���� ��ȭ")]
	public bool isSmall = true;
	public bool isMedium = false;
	public bool isLarge = false;

	[Header("��ȭ�� ����")]
	public Image evol_front;
	public Image MImage;
	public Image LImage;

	[Header("��ȭ ����")]
	public float _curEvol = 0f;
	public float _maxEvol = 20f;
	public int playerstate = 0;

	PlayerController playerController;
	SpawnManager spawnManager;

	[Header("���� �޴���")]
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

		// ��ȭ ����Ʈ�� 10 �̻��̸� ��ȭ
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

	// MediumFish�� ��ȭ
	private void EvolveToMediumFish()
	{
		smallFish.SetActive(false);
		mediumFish.SetActive(true);
		fish = mediumFish;
		playerstate = 1;
		MImage.color = new Color32(255, 255, 255, 255);
	}

	// LargeFish�� ��ȭ
	private void EvolveToLargeFish()
	{
		mediumFish.SetActive(false);
		largeFish.SetActive(true);
		fish = largeFish;
		playerstate = 2;
		LImage.color = new Color32(255, 255, 255, 255);
	}

	// ���� ���� ����
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
