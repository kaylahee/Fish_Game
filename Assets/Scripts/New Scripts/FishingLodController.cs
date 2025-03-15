using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLodController : MonoBehaviour
{
    // 내려갈 수 있는 제한선
    [SerializeField]
    private float limitY;

    private float downSpeed = 0.5f;

    private bool isDetecting = false;
    private float DetectTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 제한선보다 위에 있을 때
        if (transform.position.y > limitY + 0.5f)
        {
            transform.position += Vector3.down * downSpeed;
            isDetecting = true;
        }

        if (isDetecting)
        {
            DetectTime += Time.deltaTime;
            // 탐지 시간이 2초 정도되면
            if (DetectTime >= 2.0f)
            {
                transform.position += Vector3.up * downSpeed;

                // 초기선으로 올라오도록
                if (transform.position.y == 9f)
                {    
                    isDetecting = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
           
    }
}
