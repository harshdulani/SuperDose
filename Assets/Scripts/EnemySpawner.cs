using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public bool spawnerDebug = false;
	public GameObject enemyPrefab;
	public float spawnWaitMin = 2f, spawnWaitMax = 4f, enemiesSpawned;
	public float playerSafeSpawnRadius = 3f;
	public AudioClip spawnSound;

	[Range(1.0f, 3.0f)]
	public float spawnVolume;

	private float newX, newY;
	private GameObject player;
	private GameController gc;

	void Start()
	{
		gc = GetComponent<GameController> ();
	}

	IEnumerator Routine()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		if(!spawnerDebug)
		{
			yield return new WaitForSeconds (3f / PlayerPrefs.GetFloat("Difficulty"));
			while (gc.playerAlive)
			{
				SpawnBasic ();
				yield return new WaitForSeconds (Random.Range(spawnWaitMin, spawnWaitMax));
			}
		}
	}

	public void SpawnBasic()
	{
		Vector3 spawnPosition;
		spawnPosition = CreateSpawnPosition ();
		Instantiate (enemyPrefab, spawnPosition, Quaternion.identity);
		transform.GetChild (0).GetComponent<AudioSource> ().PlayOneShot (spawnSound, spawnVolume);
		enemiesSpawned++;
	}

	Vector3 CreateSpawnPosition()
	{
		float multiplier = 1f;
		//spawn system

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
		return position;
	}

	bool SafetyCheck(Vector3 safetyCheck)
	{
		if ((safetyCheck.x >= playerSafeSpawnRadius && safetyCheck.x <= -playerSafeSpawnRadius))
		{
			if ((safetyCheck.y >= playerSafeSpawnRadius && safetyCheck.y <= -playerSafeSpawnRadius))
				return true;
			else
				return false;
		}
		else if ((safetyCheck.y >= playerSafeSpawnRadius && safetyCheck.y <= -playerSafeSpawnRadius))
		{
			if ((safetyCheck.x >= playerSafeSpawnRadius && safetyCheck.x <= -playerSafeSpawnRadius))
				return true;
			else
				return false;
		}
		else
			return false;
	}

	void Spawn()
	{
		Vector3 spawnPosition, safetyCheck = new Vector3();
		int i = 0;
		while(true)
		{
			spawnPosition = CreateSpawnPosition ();
			if (player)
				safetyCheck = -(spawnPosition - player.transform.position);
			/*print("prospect at " + spawnPosition);
			print("player at" + player.transform.position);
			print("safety" + safetyCheck);
			print(++i);*/
			if (SafetyCheck (safetyCheck) == true) 
			{
				print ("success");
				break;
			}
			/*if((safetyCheck.x >= playerSafeSpawnRadius && safetyCheck.x <= -playerSafeSpawnRadius) && (safetyCheck.y >= playerSafeSpawnRadius && safetyCheck.y <= -playerSafeSpawnRadius))
			{
				print ("success");
				break;
			}*/
			if (i > 500) 
			{
				print ("failure");
				break;
			}
		}
		print ("spawned after " + i + "attempts at " + safetyCheck + "distance.");
		Instantiate (enemyPrefab, spawnPosition, Quaternion.identity);
		enemiesSpawned++;
	}
}

/*
	old spawn system, spawns from 4 corners
	if (Random.value <= 0.5f) 
	{
		newX = -playArea.xMax + 0.5f;
		newY = -playArea.yMax + 0.5f;
		multiplier = 1f;
	}
	else 
	{
		newX = playArea.xMax - 0.5f;
		newY = playArea.yMax - 0.5f;
		multiplier = -1f;
	}*/

/*ultra random spawn system
newX = Random.Range (-playArea.xMax + 0.25f, playArea.xMax - 0.25f);
newY = Random.Range (-playArea.yMax + 0.25f, playArea.yMax - 0.25f);
*/