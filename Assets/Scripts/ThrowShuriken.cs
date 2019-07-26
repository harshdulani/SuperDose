using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowShuriken : MonoBehaviour 
{
	public GameObject shuriken;
	public float throwSpeed = 800f;

	private GameObject player, spawnSpace;

	void Awake()
	{
		spawnSpace = transform.GetChild (0).gameObject;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			player = GameObject.FindGameObjectWithTag ("Player");
			Aim();
		}
	}

	void Aim()
	{
		GameObject shurikenInstance = Instantiate (shuriken, spawnSpace.transform.position, Quaternion.identity);

		var direction = player.transform.position - shurikenInstance.transform.position;
		float angle = (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
		shurikenInstance.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

		shurikenInstance.GetComponent<Rigidbody2D> ().AddForce (-transform.up * throwSpeed);
	}
	void OnCollisionEnter2D(Collision2D other)
	{
		print ("Boss collided with " + other);
	}
}
