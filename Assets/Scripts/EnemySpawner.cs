using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private StageData stageData; // 적 생성을 위한 스테이지 크기 정보
    [SerializeField]
    private GameObject enemyPrefab; // 복제해서 생성할 적 캐릭터 프리팹
    [SerializeField] 
    private float spawnTime; // 생성 주기 

    private AroundWrap aroundWrap;

    private void Awake()
    {
        StartCoroutine("SpawnEnemy");

        aroundWrap = GetComponent<AroundWrap>();
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            // x 위치는 스테이지의 크기 범위 내에서 임의의 값을 선택
            float positionX = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);
            // 적 캐릭터 생성
            Instantiate(enemyPrefab, new Vector3(positionX, stageData.LimitMax.y + 1.0f, 0.0f), Quaternion.identity);
            // spawntime만큼 대기
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
