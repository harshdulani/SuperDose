using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpShots : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.collider.CompareTag("Walls"))
		{
			Destroy (gameObject);
		}
	}
}
