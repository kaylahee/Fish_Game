using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
	// HP �������� �����ϴ� ����Ʈ
	public List<GameObject> hp;

	private int previousMaxHp = 0;
	private int previousCurHp = 0;

	PlayerController controller;
	GameSceneManager gameSceneManager;
	ScoreViewer scoreViewer;

	private void Start()
	{
		controller = GetComponent<PlayerController>();
		gameSceneManager = FindObjectOfType<GameSceneManager>(); 
		scoreViewer = FindObjectOfType<ScoreViewer>();

		// �ʱ� HP ���� ����
		UpdateHPStatus();
	}

	// Update is called once per frame
	private void Update()
	{
		// HP ��ȭ�� ���� ���� ������Ʈ�ϵ��� ����ȭ
		if (controller._maxHp != previousMaxHp || controller._curHp != previousCurHp)
		{
			UpdateHPStatus();
		}
	}

	// HP ���� ������Ʈ �޼ҵ�
	private void UpdateHPStatus()
	{
		// ��ȭ�� ���� maxHp ����
		for (int i = 0; i < hp.Count; i++)
		{
			// HP �������� �ִ� HP�� �°� Ȱ��ȭ
			if (i < controller._maxHp)
			{
				hp[i].SetActive(true);
			}
			else
			{
				hp[i].SetActive(false);
			}
		}

		// �������� ���� curHp ���� ������Ʈ
		if (controller._curHp == 0)
		{
			// HP�� 0�̸� ��� ������ ��Ȱ��ȭ
			for (int i = 0; i < hp.Count; i++)
			{
				hp[i].SetActive(false);
			}
			gameSceneManager.LoadScene("EndScene", scoreViewer.score);
		}
		else
		{
			// HP�� �°� Ȱ��ȭ
			for (int i = 0; i < hp.Count; i++)
			{
				if (i < controller._curHp)
					hp[i].SetActive(true);
				else
					hp[i].SetActive(false);
			}
		}

		// ���� ���� ������Ʈ
		previousMaxHp = controller._maxHp;
		previousCurHp = controller._curHp;
	}
}
