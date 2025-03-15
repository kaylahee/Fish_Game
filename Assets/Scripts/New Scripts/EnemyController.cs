using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	/*public int scorePoint = 50;

	public float currentHP;
	private PlayerHP playerHP;

	[SerializeField]
	private StageData stageData;*/

	private Rigidbody2D rg;

	public int enemyStage;

	// Start is called before the first frame update
	void Start()
	{
		rg = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void FixedUpdate()
	{
		if (gameObject.CompareTag("Feed"))
		{
			if (transform.position.x >= 10f)
			{
				rg.velocity = new Vector2(-1, rg.velocity.x);
			}
			else if (transform.position.x <= -10f)
			{
				rg.velocity = new Vector2(1, rg.velocity.x);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		
	}
}