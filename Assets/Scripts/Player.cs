using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject shot;
	public Transform spawnSpace;
	public float shotSpeed = 10f;
	public float moveSpeed = 500f;

	private float movement = 0f;

	void Update ()
	{
		movement = Input.GetAxisRaw ("Horizontal");

		if (Input.GetButtonDown ("Fire1")) {
			GameObject shotInstance =
				Instantiate (shot, spawnSpace.position, spawnSpace.rotation);
			shotInstance.GetComponent<Rigidbody2D> ().AddForce (transform.up * shotSpeed);
			Destroy (shotInstance, 1f);
		}
	}

	void FixedUpdate()
	{
		transform.RotateAround (Vector3.zero, Vector3.back, movement * Time.deltaTime * moveSpeed);
	}
}
