using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField]
    private int damage3 = 10;
    [SerializeField]
    private int scorePoint_3 = 300;
    private int scorePoint3 = 500;
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
            if (playerController.Score < 5000)
            {
                collision.GetComponent<PlayerHP>().TakeDamage(damage3);
                playerController.Score -= scorePoint_3;
                Destroy(gameObject);
            }
            else
            {
                OnDie();
            }
        }
    }
    public void OnDie()
    {
        if (playerController.Score >= 5000)
        {
            playerController.Score += scorePoint3;
            playerHP.IncreaseHp(50);
        }
        Destroy(gameObject);
    }
}

