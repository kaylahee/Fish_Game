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
		// 낚싯줄과 닿았을 경우
		if (collision.CompareTag("Hook"))
		{
			isCaught = true;
		}

		// 플레이어가 낚싯줄에 걸리지 않은 상황에서
		if (!isCaught)
		{
			// 먹이와 닿았을 경우
			if (collision.CompareTag("Feed"))
			{
				enemyController = collision.GetComponent<EnemyController>();

				// 낚싯줄에 걸리지 않은 먹이일 경우
				if (!collision.gameObject.name.Contains("Caught"))
				{
					// 플레이어의 레벨이 먹이의 레벨보다 낮을 경우
					if (evolutionController.playerstate < enemyController.enemyStage)
					{
						ifMeetEnemy();
					}
					// 플레이어의 레벨이 먹이의 레벨보다 높거나 같은 경우
					else
					{
						// 먹이 삭제
						Destroy(collision.gameObject);

						// 먹은 먹이 계산
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

						// 진화 포인트 증가
						evolutionController.AddEvolPoint(enemyController);
					}
				}
			}
			
			// 잠수부와 닿았을 경우
			if (collision.CompareTag("Swimmer"))
			{
				playerHPManager._curHp = 0;
				playerHPManager.UpdateHPStatus();
			}
			
			// 쓰레기와 닿았을 경우
			if (collision.CompareTag("Trash"))
			{
				ifMeetEnemy();
			}
		}
	}

	// 먹이, 잠수부, 쓰레기 등과 충돌할 경우
	private void ifMeetEnemy()
	{
		// 체력 감소
		playerHPManager._curHp -= 1;
		playerHPManager.UpdateHPStatus();

		// 무적 상태 전환
		StartCoroutine(UnBeatTime());
	}

	// 먹이와 닿았을 경우 무적 시간
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
