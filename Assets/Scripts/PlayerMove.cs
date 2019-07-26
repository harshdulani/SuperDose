using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	/*
	 * velocity ke liye 7.5f
	 * force ke liye 120f is good
	*/
	public float moveSpeed = 7.5f;
	private PlayArea playArea;

	void Start()
	{
		playArea = GameObject.FindWithTag ("GameController").GetComponent<GameController> ().playArea;
	}

	void Update ()
	{
		float moveH = Input.GetAxis ("Horizontal") * moveSpeed;
		float moveV = Input.GetAxis ("Vertical") * moveSpeed;

		GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveH, moveV);
		//GetComponent<Rigidbody2D> ().AddForce (new Vector2 (moveH, moveV));

		//clamp location to play area
		transform.position = new Vector3
		(
			Mathf.Clamp (transform.position.x, -playArea.xMax, playArea.xMax),
			Mathf.Clamp (transform.position.y, -playArea.yMax, playArea.yMax),
			0f
		);
	}
}
