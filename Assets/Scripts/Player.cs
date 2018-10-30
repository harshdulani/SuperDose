using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float moveSpeed = 500f;

	float movement = 0f;

	void Update ()
	{
		movement = Input.GetAxisRaw ("Horizontal");
	}

	void FixedUpdate()
	{
		transform.RotateAround (Vector3.zero, Vector3.back, movement * Time.deltaTime * moveSpeed);
	}
}
