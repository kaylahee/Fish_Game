using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private int scorePoint = 100;
    private PlayerController playerController;
    private int getHP = 10;
    private PlayerHP playerHP;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (scorePoint >= 2000)
        {
            playerHP.currentHP += getHP;
        }

        OnDie();
    }

    public void OnDie()
    {
        playerController.Score += scorePoint;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHP>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

