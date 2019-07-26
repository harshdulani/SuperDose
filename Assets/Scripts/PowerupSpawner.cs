using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
	//this is on the GC
	//this decides spawn location and spawns it.
	//the PowerupSelector script on the Powerup Prefab decides which powerup and sprite management
	public float startWait = 5.5f, forceMultiplier = 10f;
	public bool pressToSpawnE;
	public GameObject powerupPrefab;

	private GameController gc;
	void Start ()
	{
		gc = GetComponent<GameController> ();
	}

	void Update()
	{
		if (Time.time - gc.powerupLastActive > Random.Range (7f, 9f) && !GameObject.FindGameObjectWithTag("Powerup") && gc.gameStarted && !gc.gameOver)
			SpawnRandomPowerup ();
		if (pressToSpawnE && Input.GetKeyDown ("e"))
			SpawnRandomPowerup ();		
	}

	void SpawnRandomPowerup()
	{
		float multiplier = 1f, newX, newY;
		//randomize multiplier value to randomize Min/Max locations
		if (Random.value <= 0.5f)
			multiplier = -1f;
		else
			multiplier = 1f;

		//spawn system
		if (Random.value > 0.5f) 
		{
			//randomize on X and not Y
			newY = multiplier * (gc.playArea.yMax - 0.25f);
			newX = Random.Range (-(gc.playArea.xMax - 0.25f), (gc.playArea.xMax - 0.25f));
		}
		else
		{
			//randomize on Y and not X
			newX = multiplier * (gc.playArea.xMax - 0.25f);
			newY = Random.Range (-(gc.playArea.yMax - 0.25f), (gc.playArea.yMax - 0.25f));
		}

		//Debug.Log ("New X = " + newX + " New Y = " + newY);
		Vector3 position = new Vector3 (newX, newY, 0f);

		GameObject powerupInstance = Instantiate (powerupPrefab, position, Quaternion.identity);
		Vector2 forceVector = new Vector2 (-position.x * forceMultiplier, -position.y * forceMultiplier);
		powerupInstance.GetComponent<Rigidbody2D> ().AddForce (forceVector);
	}
}
