using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
	private SpriteRenderer cur_player;

	private bool isCaught = false;
	private bool isUnBeatTime;

	PlayerController playerController;
	EvolutionController evolutionController;
	PlayerHPManager playerHPManager;
	
	EnemyController enemyController;

	public int eatFeed1Count = 0;
	public int eatFeed2Count = 0;
	public int eatFeed3Count = 0;

	private void Start()
	{
		evolutionController = GetComponentInParent<EvolutionController>();
		cur_player = evolutionController.cur_player;

		playerController = GetComponentInParent<PlayerController>();
		playerHPManager = GetComponentInParent<PlayerHPManager>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �����ٰ� ����� ���
		if (collision.CompareTag("Hook"))
		{
			isCaught = true;
		}

		// �÷��̾ �����ٿ� �ɸ��� ���� ��Ȳ����
		if (!isCaught)
		{
			// ���̿� ����� ���
			if (collision.CompareTag("Feed"))
			{
				enemyController = collision.GetComponent<EnemyController>();

				// �����ٿ� �ɸ��� ���� ������ ���
				if (!collision.gameObject.name.Contains("Caught"))
				{
					// �÷��̾��� ������ ������ �������� ���� ���
					if (evolutionController.playerstate < enemyController.enemyStage)
					{
						ifMeetEnemy();
					}
					// �÷��̾��� ������ ������ �������� ���ų� ���� ���
					else
					{
						// ���� ����
						Destroy(collision.gameObject);

						// ���� ���� ���
						if (enemyController.enemyStage == 0)
						{
							eatFeed1Count++;
						}
						else if (enemyController.enemyStage == 1)
						{
							eatFeed2Count++;
						}
						else if (enemyController.enemyStage == 2)
						{
							eatFeed3Count++;
						}

						// ��ȭ ����Ʈ ����
						evolutionController.AddEvolPoint(enemyController);
					}
				}
			}
			// ����ο� ����� ���
			else if (collision.CompareTag("Swimmer"))
			{
				playerHPManager._curHp = 0;
				playerHPManager.UpdateHPStatus();
			}
			// ������� ����� ���
			else if (collision.CompareTag("Trash"))
			{
				ifMeetEnemy();
			}
		}
	}

	// ����, �����, ������ ��� �浹�� ���
	private void ifMeetEnemy()
	{
		// ü�� ����
		playerHPManager._curHp -= 1;
		playerHPManager.UpdateHPStatus();

		// ���� ���� ��ȯ
		isUnBeatTime = true;
		StartCoroutine(UnBeatTime());
	}

	// ���̿� ����� ��� ���� �ð�
	private IEnumerator UnBeatTime()
	{
		int countTime = 0;
		while (countTime < 10)
		{
			playerController.Movespeed = 1f;
			if (countTime % 2 == 0)
			{
				cur_player.color = new Color32(255, 255, 255, 90);
			}
			else
			{
				cur_player.color = new Color32(255, 255, 255, 180);
			}

			yield return new WaitForSeconds(0.2f);
			countTime++;
		}
		cur_player.color = new Color32(255, 255, 255, 255);
		playerController.Movespeed = 3f;
		isUnBeatTime = false;

		yield return null;
	}
}
