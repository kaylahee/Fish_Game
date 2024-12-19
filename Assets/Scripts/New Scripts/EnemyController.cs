using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	/*public int scorePoint = 50;

	public float currentHP;
	private PlayerHP playerHP;

	[SerializeField]
	private StageData stageData;*/

	private PlayerController playerController;

	public float moveSpeed = 1.0f;

	// Start is called before the first frame update
	void Start()
	{
		//playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		/*Debug.Log($"{gameObject.name}, {transform.rotation.y}");

		if (transform.rotation.y == -1)
		{
			transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
		}
		else
		{
			transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
		}*/

		if (transform.position.y > 3.5f)
		{
			transform.position += Vector3.down * moveSpeed;
		}
	}

	/*void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			playerController.score += scorePoint;
			Destroy(gameObject);
		}
	}*/
}
