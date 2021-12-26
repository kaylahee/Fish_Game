using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField]
    private int damage2 = 3;
    [SerializeField]
    private int scorePoint_2 = 50;
    private int scorePoint2 = 100;
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
            if (playerController.Score < 1000)
            {
                collision.GetComponent<PlayerHP>().TakeDamage(damage2);
                playerController.Score -= scorePoint_2;
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
        if (playerController.Score >= 1000)
        {
            playerController.Score += scorePoint2;
            playerHP.IncreaseHp(25);
        }
        Destroy(gameObject);
    }
}
    
