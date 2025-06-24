using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
	private SpriteRenderer cur_player;

	[HideInInspector]
	public bool isCaught = false;

	PlayerController playerController;
	EvolutionController evolutionController;
	PlayerHPManager playerHPManager;
	
	EnemyController enemyController;
		
	private void Start()
	{
		playerController = GetComponentInParent<PlayerController>();
		evolutionController = GetComponentInParent<EvolutionController>();
		playerHPManager = GetComponentInParent<PlayerHPManager>();

		cur_player = GetComponent<SpriteRenderer>();
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
							playerController.eatFeed1Count++;
						}
						else if (enemyController.enemyStage == 1)
						{
							playerController.eatFeed2Count++;
						}
						else if (enemyController.enemyStage == 2)
						{
							playerController.eatFeed3Count++;
						}

						// ��ȭ ����Ʈ ����
						evolutionController.AddEvolPoint(enemyController);
					}
				}
			}
			
			// ����ο� ����� ���
			if (collision.CompareTag("Swimmer"))
			{
				playerHPManager._curHp = 0;
				playerHPManager.UpdateHPStatus();
			}
			
			// ������� ����� ���
			if (collision.CompareTag("Trash"))
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
		StartCoroutine(UnBeatTime());
	}

	// ���̿� ����� ��� ���� �ð�
	private IEnumerator UnBeatTime()
	{
		int countTime = 0;
		while (countTime < 10)
		{
			gameObject.GetComponent<CircleCollider2D>().enabled = false;
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
		gameObject.GetComponent<CircleCollider2D>().enabled = true;
		playerController.Movespeed = 3f;
	}
}
