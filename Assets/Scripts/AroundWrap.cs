using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundWrap : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;

    public void UpdateAroundWrap()
    {
        Vector3 position = transform.position;

        // ���� ���̳� ������ ���� �����ϸ� �ݴ������� �̵�
        if (position.x < stageData.LimitMin.x || position.x > stageData.LimitMax.x)
        {
            position.x *= -1;
        }

        // ���� ���̳� �Ʒ��� ���� �����ϸ� �ݴ������� �̵�
        if (position.y < stageData.LimitMin.y || position.y > stageData.LimitMin.y)
        {
            position.y *= -1;
        }

        transform.position = position;
    }
}
