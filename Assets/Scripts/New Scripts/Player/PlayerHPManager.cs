using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHPManager : MonoBehaviour
{
	[Header("ü�� ����")]
	public int _curHp = 1;
	public int _maxHp = 1;

	[Header("ü�� ������")]
	public List<GameObject> hp;

	private bool isEvol_StoM = false;
	private bool isEvol_MtoL = false;

	EvolutionController evolutionController;

	public GameObject gameManager;
	GameSceneManager gameSceneManager;

	private void Start()
	{
		evolutionController = GetComponent<EvolutionController>();
		gameSceneManager = gameManager.GetComponent<GameSceneManager>();

		// _maxHp ��ŭ ������ active
		for (int i = 0; i < _maxHp; i++)
		{
			hp[i].SetActive(true);
		}
	}

	// Update is called once per frame
	private void Update()
	{
		if (_curHp == 0)
		{	
			gameSceneManager.LoadScene("EndScene");
		}

		if (isEvol_StoM && evolutionController.playerstate == 1)
		{
			_maxHp++;
			_curHp++;
			UpdateHPStatus();
			isEvol_StoM = false;
		}

		if (isEvol_MtoL && evolutionController.playerstate == 2)
		{
			_maxHp++;
			_curHp++;
			UpdateHPStatus();
			isEvol_MtoL = false;
		}
	}

	// HP ���� ������Ʈ
	public void UpdateHPStatus()
	{
		// �ִ� ü�� ����ŭ ������ Ȱ��ȭ
		for (int i = 0; i < _maxHp; i++)
		{
			hp[i].SetActive(true);
		}
	}
}
