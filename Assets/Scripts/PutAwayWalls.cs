using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutAwayWalls : MonoBehaviour
{
	public bool tempGameOver;

	void Update ()
	{
		tempGameOver = GameObject.FindWithTag ("GameController").GetComponent<GameController>().gameOver;
		if(!tempGameOver && GameObject.FindWithTag("Player"))
		{
			if(name == "Left")
				StartCoroutine (WallsAwayLeft());
			else
				StartCoroutine (WallsAwayRight());
		}
	}

	IEnumerator WallsAwayLeft()
	{
		while(transform.position.x > -12f)
		{
			transform.position = new Vector3 
			(
				transform.position.x - 0.05f,
				transform.position.y,
				0f
			);
			yield return new WaitForSeconds (0.1f);
		}
	}

	IEnumerator WallsAwayRight()
	{
		while(transform.position.x < 12f)
		{
			transform.position = new Vector3 
				(
					transform.position.x + 0.05f,
					transform.position.y,
					0f
				);
			yield return new WaitForSeconds (0.1f);
		}
	}
}
