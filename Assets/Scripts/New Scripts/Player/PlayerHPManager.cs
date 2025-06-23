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

		for (int i = 0; i < _curHp; i++)
		{
			hp[i].SetActive(true);
			hp[i].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
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

		// ���� ü�� ����ŭ ��� ó��
		for (int i = 0; i < _curHp; i++)
		{
			hp[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
		}

		// ���� ü���� ȸ�� ó��
		for (int i = _curHp; i < _maxHp; i++)
		{
			hp[i].GetComponent<SpriteRenderer>().color = new Color(0.623f, 0.623f, 0.623f);
		}
	}
}
