using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private StageData stageData; // �� ������ ���� �������� ũ�� ����
    [SerializeField]
    private GameObject enemyPrefab; // �����ؼ� ������ �� ĳ���� ������
    [SerializeField] 
    private float spawnTime; // ���� �ֱ� 

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
            // x ��ġ�� ���������� ũ�� ���� ������ ������ ���� ����
            float positionX = Random.Range(stageData.LimitMin.x, stageData.LimitMax.x);
            // �� ĳ���� ����
            Instantiate(enemyPrefab, new Vector3(positionX, stageData.LimitMax.y + 1.0f, 0.0f), Quaternion.identity);
            // spawntime��ŭ ���
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
