using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private StageData stageData; // 적 생성을 위한 스테이지 크기 정보
    [SerializeField]
    private GameObject[] enemyPrefab; // 복제해서 생성할 적 캐릭터 프리팹
    [SerializeField] 
    private float spawnTime; // 생성 주기 

    private PlayerController player;
    public Transform[] spawnPoint;

    //private AroundWrap aroundWrap;

    void Start()
    {
		player = FindObjectOfType<PlayerController>();

		StartCoroutine(SpawnEnemy());
        //aroundWrap = GetComponent<AroundWrap>();
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
			for (int i = 0; i < enemyPrefab.Length; i++)
			{
				float positionY = Random.Range(stageData.LimitMin.y, stageData.LimitMax.y);
				//Debug.Log(enemyPrefab[i].name);
				if (enemyPrefab[i].tag == "Level0")
				{
					//Debug.Log("in");
					//Debug.Log(enemyPrefab[i].name);
					// 적 캐릭터 생성
					Instantiate(enemyPrefab[i], new Vector2(spawnPoint[i].position.x, positionY), enemyPrefab[i].transform.rotation);
				}

				if (enemyPrefab[i].tag == "Level1" && player.score >= 100)
				{
					//Debug.Log("in");
					//Debug.Log(enemyPrefab[i].name);
					// 적 캐릭터 생성
					Instantiate(enemyPrefab[i], new Vector2(spawnPoint[i].position.x, positionY), enemyPrefab[i].transform.rotation);
					spawnTime += 5;
				}

				if (enemyPrefab[i].tag == "Level2" && player.score >= 200)
				{
					//Debug.Log("in");
					//Debug.Log(enemyPrefab[i].name);
					// 적 캐릭터 생성
					Instantiate(enemyPrefab[i], new Vector2(spawnPoint[i].position.x, positionY), enemyPrefab[i].transform.rotation);
				}

				if (enemyPrefab[i].tag == "Level3" && player.score >= 300)
				{
					//Debug.Log("in");
					//Debug.Log(enemyPrefab[i].name);
					// 적 캐릭터 생성
					Instantiate(enemyPrefab[i], new Vector2(spawnPoint[i].position.x, positionY), enemyPrefab[i].transform.rotation);
				}
			}

			// spawntime만큼 대기
			yield return new WaitForSeconds(spawnTime);
        }
    }


}
