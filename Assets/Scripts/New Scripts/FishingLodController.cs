using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingLodController : MonoBehaviour
{
    // ������ �� �ִ� ���Ѽ�
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
        // ���Ѽ����� ���� ���� ��
        if (transform.position.y > limitY + 0.5f)
        {
            transform.position += Vector3.down * downSpeed;
            isDetecting = true;
        }

        if (isDetecting)
        {
            DetectTime += Time.deltaTime;
            // Ž�� �ð��� 2�� �����Ǹ�
            if (DetectTime >= 2.0f)
            {
                transform.position += Vector3.up * downSpeed;

                // �ʱ⼱���� �ö������
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
