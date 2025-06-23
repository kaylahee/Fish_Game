using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolBarViewer : MonoBehaviour
{
	public Image evol_front;
	private float _maxEvol = 20f;

	PlayerController playerController;

	void Start()
	{
		playerController = FindObjectOfType<PlayerController>();
		evol_front.GetComponent<Image>().fillAmount = 0f;
	}

	void Update()
	{
		evol_front.fillAmount = playerController._curEvol / _maxEvol;
	}
}