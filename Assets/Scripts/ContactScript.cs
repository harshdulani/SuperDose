using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactScript : MonoBehaviour 
{
	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.collider.CompareTag("Shot"))
		{
			Destroy (other.gameObject);
			//some method to increase charge to kill boss
		}
		print ("Shuriken collided with " + other.gameObject);
		Destroy (gameObject);
	}
}
