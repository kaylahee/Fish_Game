using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField]
    private int scorePoint1 = 50;
    private PlayerController playerController;
    public float currentHP;
    private PlayerHP playerHP;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerHP = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHP>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerController.Score += scorePoint1;
            playerHP.IncreaseHp(1);
            Destroy(gameObject);
        }
    }
}
