using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
	// HP 아이콘을 저장하는 리스트
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

		// 초기 HP 상태 설정
		UpdateHPStatus();
	}

	// Update is called once per frame
	private void Update()
	{
		// HP 변화가 있을 때만 업데이트하도록 최적화
		if (controller._maxHp != previousMaxHp || controller._curHp != previousCurHp)
		{
			UpdateHPStatus();
		}
	}

	// HP 상태 업데이트 메소드
	private void UpdateHPStatus()
	{
		// 진화에 따른 maxHp 변경
		for (int i = 0; i < hp.Count; i++)
		{
			// HP 아이콘을 최대 HP에 맞게 활성화
			if (i < controller._maxHp)
			{
				hp[i].SetActive(true);
			}
			else
			{
				hp[i].SetActive(false);
			}
		}

		// 데미지에 따른 curHp 상태 업데이트
		if (controller._curHp == 0)
		{
			// HP가 0이면 모든 아이콘 비활성화
			for (int i = 0; i < hp.Count; i++)
			{
				hp[i].SetActive(false);
			}
			gameSceneManager.LoadScene("EndScene", scoreViewer.score);
		}
		else
		{
			// HP에 맞게 활성화
			for (int i = 0; i < hp.Count; i++)
			{
				if (i < controller._curHp)
					hp[i].SetActive(true);
				else
					hp[i].SetActive(false);
			}
		}

		// 이전 상태 업데이트
		previousMaxHp = controller._maxHp;
		previousCurHp = controller._curHp;
	}
}
