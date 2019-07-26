using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutAwayWalls : MonoBehaviour
{
	public float waitForTime = 0.001f;
	public void WallManager(bool input)
	{
		if(input)
		{
			if(name == "Left")
				StartCoroutine (WallsAwayLeft());
			else if(name == "Right")
				StartCoroutine (WallsAwayRight());
			else if(name == "Top")
				StartCoroutine (WallsAwayTop());
			else if(name == "Bottom")
				StartCoroutine (WallsAwayBottom());
		}
		else
		{
			if(name == "Left")
				StartCoroutine (WallsBackLeft());
			else if(name == "Right")
				StartCoroutine (WallsBackRight());
			else if(name == "Top")
				StartCoroutine (WallsBackTop());
			else if(name == "Bottom")
				StartCoroutine (WallsBackBottom());
		}
	}

	IEnumerator WallsAwayLeft()
	{
		while(transform.position.x > -13.5f)
		{
			transform.position = new Vector3 
				(
					transform.position.x - 0.25f,
					transform.position.y,
					0f
				);
			yield return new WaitForSeconds (waitForTime);
		}
	}

	IEnumerator WallsAwayRight()
	{
		while(transform.position.x < 13.5f)
		{
			transform.position = new Vector3 
				(
					transform.position.x + 0.25f,
					transform.position.y,
					0f
				);
			yield return new WaitForSeconds (waitForTime);
		}
	}

	IEnumerator WallsAwayBottom()
	{
		while(transform.position.y > -7.5f)
		{
			transform.position = new Vector3 
			(
				transform.position.x,
				transform.position.y - 0.25f,
				0f
			);
			yield return new WaitForSeconds (waitForTime);
		}
	}
		
	IEnumerator WallsAwayTop()
	{
		while(transform.position.y < 7.5f)
		{
			transform.position = new Vector3 
				(
					transform.position.x,
					transform.position.y + 0.25f,
					0f
				);
			yield return new WaitForSeconds (waitForTime);
		}
	}

	IEnumerator WallsBackLeft()
	{
		while(transform.position.x < -7.5f)
		{
			transform.position = new Vector3 
				(
					transform.position.x + 0.25f,
					transform.position.y,
					0f
				);
			yield return new WaitForSeconds (waitForTime);
		}
	}

	IEnumerator WallsBackRight()
	{
		while(transform.position.x > 7.5f)
		{
			transform.position = new Vector3 
				(
					transform.position.x - 0.25f,
					transform.position.y,
					0f
				);
			yield return new WaitForSeconds (waitForTime);
		}
	}

	IEnumerator WallsBackBottom()
	{
		while(transform.position.y < -5.3f)
		{
			transform.position = new Vector3 
				(
					transform.position.x,
					transform.position.y + 0.25f,
					0f
				);
			yield return new WaitForSeconds (waitForTime);
		}
	}

	IEnumerator WallsBackTop()
	{
		while(transform.position.y > 5.3f)
		{
			transform.position = new Vector3 
				(
					transform.position.x,
					transform.position.y - 0.25f,
					0f
				);
			yield return new WaitForSeconds (waitForTime);
		}
	}
}
