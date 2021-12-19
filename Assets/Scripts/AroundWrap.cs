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

        // 왼쪽 끝이나 오른쪽 끝에 도달하면 반대편으로 이동
        if (position.x < stageData.LimitMin.x || position.x > stageData.LimitMax.x)
        {
            position.x *= -1;
        }

        // 위쪽 끝이나 아래쪽 끝에 도달하면 반대편으로 이동
        if (position.y < stageData.LimitMin.y || position.y > stageData.LimitMin.y)
        {
            position.y *= -1;
        }

        transform.position = position;
    }
}
