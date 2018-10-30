using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour 
{
	public GameObject enemyExplosion;


	void OnCollisionEnter2D (Collision2D other)
	{
		GameController gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController> ();
		EnemyLevelManager elm = GetComponent<EnemyLevelManager> ();

		//Debug.Log ("Shot Collides with " + other.collider.tag);
		if (other.collider.CompareTag ("Shot")) 
		{
			Destroy (other.gameObject);
			elm.health -= gameController.healthHit;
			if (elm.health <= (elm.maxHealth / 2))
				elm.HealthLow ();

			if(elm.health <= 0f)
			{
				Destroy (Instantiate (enemyExplosion, transform.position, transform.rotation), 3.5f);
				gameController.EnemyKilled (elm.enemyLevel);
				Destroy (gameObject);
			}
		}
		if(other.collider.CompareTag ("Player"))
		{
			Destroy (other.gameObject);
			//send message to THIS enemy prefab's grow shrink function and make it bigger
			gameController.playerAlive = false;
			gameController.enemiesKilled++;
			//print (GameObject.FindWithTag ("GameController").GetComponent<GameController> ().enemiesKilled);
		}

		if(other.collider.CompareTag("Enemy"))
		{
			//kill that one and make this one larger (prefereably 2x) also give this full 2x health
			if (other.gameObject.GetComponent<EnemyLevelManager> ().enemyLevel == elm.enemyLevel)
			{
				if(elm.enemyLevel <= 3)
					gameController.FuseEnemy (gameObject, other.gameObject);
			}
		}
	}
}
