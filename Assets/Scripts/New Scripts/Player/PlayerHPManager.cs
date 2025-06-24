using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHPManager : MonoBehaviour
{
	[Header("체력 관리")]
	public int _curHp = 1;
	public int _maxHp = 1;

	[Header("체력 아이콘")]
	public List<GameObject> hp;

	private bool isEvol_StoM = false;
	private bool isEvol_MtoL = false;

	EvolutionController evolutionController;
	InteractionController interactionController;

	public GameObject gameManager;
	GameSceneManager gameSceneManager;

	private void Start()
	{
		evolutionController = GetComponent<EvolutionController>();
		interactionController = evolutionController.fish.GetComponent<InteractionController>();
		gameSceneManager = gameManager.GetComponent<GameSceneManager>();

		// _maxHp 만큼 아이콘 active
		for (int i = 0; i < _maxHp; i++)
		{
			hp[i].SetActive(true);
		}

		for (int i = 0; i < _curHp; i++)
		{
			hp[i].SetActive(true);
			hp[i].GetComponent<Image>().color = new Color(255, 255, 255);
		}
	}

	// Update is called once per frame
	private void Update()	
	{
		if (!gameObject.name.Contains("Caught") && _curHp == 0)
		{
			gameSceneManager.LoadScene("EndScene");
		}

		if (!isEvol_StoM && evolutionController.playerstate == 1)
		{
			_maxHp++;
			_curHp++;
			UpdateHPStatus();
			isEvol_StoM = true;
		}

		if (!isEvol_MtoL && evolutionController.playerstate == 2)
		{
			_maxHp++;
			_curHp++;
			UpdateHPStatus();
			isEvol_MtoL = true;
		}
	}

	// HP 상태 업데이트
	public void UpdateHPStatus()
	{
		// 최대 체력 수만큼 아이콘 활성화
		for (int i = 0; i < _maxHp; i++)
		{
			hp[i].SetActive(true);
		}

		// 현재 체력 수만큼 흰색 처리
		for (int i = 0; i < _curHp; i++)
		{
			hp[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
		}

		// 남은 체력은 회색 처리
		for (int i = _curHp; i < _maxHp; i++)
		{
			hp[i].GetComponent<Image>().color = new Color(0.623f, 0.623f, 0.623f);
		}
	}
}
