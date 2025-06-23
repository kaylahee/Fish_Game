using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	[Header("���� ����� �ӵ�")]
    public float Movespeed = 3f;

	EvolutionController evolutionController;

	private GameObject curFish;

	private void Start()
    {
		evolutionController = GetComponent<EvolutionController>();
	}

    private void Update()
    {
		curFish = evolutionController.fish;
		// ���� ����Ⱑ �ƴ� ���
		if (!gameObject.name.Contains("Caught"))
		{
			//MoveInArea();
			Move();
		}
	}

    // ���� Ű�� ���� �̵� ���� ����
    private void Move()
    {
        // ���� �̵�
        float userInputV = Input.GetAxis("Vertical");
        // ���� �̵�
        float userInputH = Input.GetAxis("Horizontal");

		Vector3 direction = new Vector3(userInputH, userInputV, 0);

		if (!(userInputV == 0 && userInputH == 0))
		{
			// �̵�
			transform.position += direction * Movespeed * Time.deltaTime;

			if (userInputH < 0f)
			{
				curFish.transform.rotation = Quaternion.Euler(0, 0, 0);

				// 3��и�
				if (userInputV < 0f)
				{
					curFish.transform.rotation = Quaternion.Euler(0, 0, 45);
				}
				// 2��и�
				else if (userInputV > 0f)
				{
					curFish.transform.rotation = Quaternion.Euler(0, 0, -45);
				}
			}
			else if (userInputH > 0f)
			{
				curFish.transform.rotation = Quaternion.Euler(0, 180, 0);

				// 4��и�
				if (userInputV < 0f)
				{
					curFish.transform.rotation = Quaternion.Euler(0, 180, 45);
				}
				// 1��и�
				else if (userInputV > 0f)
				{
					curFish.transform.rotation = Quaternion.Euler(0, 180, -45);
				}
			}
			else if (userInputV < 0f)
			{
				curFish.transform.rotation = Quaternion.Euler(0, 0, 90);
			}
			else if (userInputV > 0f)
			{
				curFish.transform.rotation = Quaternion.Euler(0, 0, -90);
			}
		}		
    }

	// ������ �� �ִ� ���� ������ �����̵��� ����
	//private void MoveInArea()
	//{
	//	// ��� ��輱 ������ �����̵���
	//	Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

	//	Vector4[] bounds = new Vector4[] {
	//		new Vector4(0.03f, 0.97f, 0.2f, 0.95f), // playerstate 0
	//		new Vector4(0.07f, 0.93f, 0.2f, 0.95f), // playerstate 1
	//		new Vector4(0.11f, 0.89f, 0.22f, 0.95f) // playerstate 2
	//	};

	//	Vector4 b = bounds[playerstate];
	//	position.x = Mathf.Clamp(position.x, b.x, b.y);
	//	position.y = Mathf.Clamp(position.y, b.z, b.w);

	//	transform.position = Camera.main.ViewportToWorldPoint(position);
	//}	
}